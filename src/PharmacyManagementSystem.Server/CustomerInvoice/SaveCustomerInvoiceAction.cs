using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventory;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.AuditLog;
using PharmacyManagementSystem.Server.Drug;
using PharmacyManagementSystem.Server.DrugInventory;
using PharmacyManagementSystem.Server.GstCalculation;
using PharmacyManagementSystem.Server.Patient;

namespace PharmacyManagementSystem.Server.CustomerInvoice;

public class SaveCustomerInvoiceAction(
    ILogger<SaveCustomerInvoiceAction> logger,
    ICustomerInvoiceRepository repository,
    IDrugRepository drugRepository,
    IDrugInventoryRepository drugInventoryRepository,
    IGstCalculationService gstCalculationService,
    ISaveAuditLogAction auditLogAction,
    IPatientRepository patientRepository) : ISaveCustomerInvoiceAction
{
    private static readonly HashSet<string> ControlledSchedules =
        new(StringComparer.OrdinalIgnoreCase) { "H", "H1", "X" };

    private readonly ILogger<SaveCustomerInvoiceAction> _logger = logger;
    private readonly ICustomerInvoiceRepository _repository = repository;
    private readonly IDrugRepository _drugRepository = drugRepository;
    private readonly IDrugInventoryRepository _drugInventoryRepository = drugInventoryRepository;
    private readonly IGstCalculationService _gstCalculationService = gstCalculationService;
    private readonly ISaveAuditLogAction _auditLogAction = auditLogAction;
    private readonly IPatientRepository _patientRepository = patientRepository;

    public async Task<Common.CustomerInvoice.CustomerInvoice?> AddAsync(
        Common.CustomerInvoice.CustomerInvoice? customerInvoice,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        if (string.IsNullOrWhiteSpace(customerInvoice.Status))
            throw new BadRequestException("CustomerInvoice Status is required.");

        List<Common.AuditLog.AuditLog> pendingAuditLogs = [];
        List<(Common.DrugInventory.DrugInventory Batch, int DeductQty)> stockDeductions = [];

        if (customerInvoice.Items.Count > 0)
        {
            (pendingAuditLogs, stockDeductions) = await ValidateAndComputeItemsAsync(customerInvoice, cancellationToken).ConfigureAwait(false);
        }

        if (customerInvoice.PatientId.HasValue && customerInvoice.PatientId.Value != Guid.Empty)
        {
            await CheckCreditLimitAsync(customerInvoice.PatientId.Value, customerInvoice.NetAmount, cancellationToken).ConfigureAwait(false);
        }

        customerInvoice.InvoiceNumber = await _repository
            .GetNextInvoiceNumberAsync(cancellationToken).ConfigureAwait(false);

        customerInvoice.UpdatedBy = "system";

        _logger.LogDebug("Adding new customer invoice {InvoiceNumber}.", customerInvoice.InvoiceNumber);

        var result = await _repository.AddAsync(customerInvoice, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added customer invoice {InvoiceNumber}.", customerInvoice.InvoiceNumber);

        if (result is not null)
        {
            foreach (var auditLog in pendingAuditLogs)
            {
                auditLog.CustomerInvoiceId = result.Id;
                await _auditLogAction.AddAsync(auditLog, cancellationToken).ConfigureAwait(false);
            }

            foreach (var (batch, qty) in stockDeductions)
            {
                batch.QuantityInStock -= qty;
                batch.UpdatedBy = "system";
                await _drugInventoryRepository.UpdateAsync(batch, cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Deducted {Qty} from batch {Batch} (DrugId {DrugId}). Remaining: {Remaining}.",
                    qty, batch.BatchNumber, batch.DrugId, batch.QuantityInStock);
            }
        }

        return result;
    }

    public async Task<Common.CustomerInvoice.CustomerInvoice?> UpdateAsync(
        Common.CustomerInvoice.CustomerInvoice? customerInvoice,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        if (string.IsNullOrWhiteSpace(customerInvoice.Status))
            throw new BadRequestException("CustomerInvoice Status is required.");

        if (customerInvoice.Items.Count > 0)
        {
            await ValidateAndComputeItemsAsync(customerInvoice, cancellationToken).ConfigureAwait(false);
        }

        customerInvoice.UpdatedBy = "system";


        _logger.LogDebug("Updating customer invoice with id: {Id}.", customerInvoice.Id);

        var result = await _repository.UpdateAsync(customerInvoice, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated customer invoice with id: {Id}.", customerInvoice.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing customer invoice with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed customer invoice with id: {Id}.", id);
    }

    private async Task CheckCreditLimitAsync(Guid patientId, decimal invoiceNetAmount, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(patientId.ToString(), cancellationToken)
            .ConfigureAwait(false);

        if (patient is null || patient.CreditLimit <= 0)
            return;

        if (invoiceNetAmount > patient.CreditLimit)
            throw new BadRequestException(
                $"Invoice total ₹{invoiceNetAmount:F2} exceeds the credit limit of ₹{patient.CreditLimit:F2} for patient '{patient.Name}'.");
    }

    private async Task<(List<Common.AuditLog.AuditLog> AuditLogs, List<(Common.DrugInventory.DrugInventory Batch, int Qty)> StockDeductions)> ValidateAndComputeItemsAsync(
        Common.CustomerInvoice.CustomerInvoice invoice,
        CancellationToken cancellationToken)
    {
        var totalCgst = 0m;
        var totalSgst = 0m;
        var totalIgst = 0m;
        var subTotal = 0m;
        var totalDiscount = 0m;
        var auditLogs = new List<Common.AuditLog.AuditLog>();
        var stockDeductions = new List<(Common.DrugInventory.DrugInventory Batch, int Qty)>();

        foreach (var item in invoice.Items)
        {
            var drug = await _drugRepository.GetByIdAsync(item.DrugId.ToString(), cancellationToken)
                .ConfigureAwait(false);

            if (drug is null)
                throw new BadRequestException($"Drug {item.DrugId} not found.");

            // MRP enforcement
            if (item.UnitPrice > drug.Mrp)
                throw new BadRequestException(
                    $"Billing above MRP is not permitted for '{drug.Name}'. MRP: {drug.Mrp:F2}, billed: {item.UnitPrice:F2}.");

            // Schedule H / H1 / X — prescription required
            if (!string.IsNullOrWhiteSpace(drug.ScheduleCategory)
                && ControlledSchedules.Contains(drug.ScheduleCategory)
                && invoice.PrescriptionId is null)
            {
                throw new BadRequestException(
                    $"Drug '{drug.Name}' is Schedule {drug.ScheduleCategory} and requires a valid prescription. Set PrescriptionId on the invoice.");
            }

            // Queue audit log entry for controlled drugs
            if (!string.IsNullOrWhiteSpace(drug.ScheduleCategory)
                && ControlledSchedules.Contains(drug.ScheduleCategory))
            {
                auditLogs.Add(new Common.AuditLog.AuditLog
                {
                    DrugId = drug.Id,
                    DrugName = drug.Name,
                    ScheduleCategory = drug.ScheduleCategory,
                    PrescriptionId = invoice.PrescriptionId,
                    PatientId = invoice.PatientId,
                    QuantityDispensed = item.Quantity,
                    PerformedBy = "system"
                });
            }

            // GST calculation
            var gst = _gstCalculationService.CalculateLineGst(
                item.UnitPrice,
                item.Quantity,
                item.DiscountPercent,
                drug.GstSlab,
                invoice.PharmacyGstin,
                invoice.PatientGstin);

            // Write back to item
            item.HsnCode ??= drug.HsnCode;
            item.GstRate = drug.GstSlab;
            item.TaxableValue = gst.TaxableValue;
            item.CgstAmount = gst.CgstAmount;
            item.SgstAmount = gst.SgstAmount;
            item.IgstAmount = gst.IgstAmount;
            item.Amount = gst.TotalAmount;

            totalCgst += gst.CgstAmount;
            totalSgst += gst.SgstAmount;
            totalIgst += gst.IgstAmount;
            subTotal += item.UnitPrice * item.Quantity;
            totalDiscount += (item.UnitPrice * item.Quantity) * (item.DiscountPercent / 100m);

            // Stock deduction plan (FIFO by expiry date)
            var batches = await _drugInventoryRepository
                .GetByFilterCriteriaAsync(new DrugInventoryFilter { DrugId = item.DrugId }, cancellationToken)
                .ConfigureAwait(false);

            var availableBatches = (batches ?? [])
                .Where(b => b.QuantityInStock > 0
                    && (string.IsNullOrWhiteSpace(item.BatchNumber) || b.BatchNumber == item.BatchNumber))
                .OrderBy(b => b.ExpirationDate)
                .ToList();

            var totalAvailable = availableBatches.Sum(b => b.QuantityInStock);
            if (totalAvailable < item.Quantity)
                throw new BadRequestException(
                    $"Insufficient stock for '{drug.Name}'. Required: {item.Quantity}, available: {totalAvailable}.");

            var remaining = item.Quantity;
            foreach (var batch in availableBatches)
            {
                if (remaining <= 0) break;
                var deduct = Math.Min(remaining, batch.QuantityInStock);
                stockDeductions.Add((batch, deduct));
                remaining -= deduct;
            }
        }

        invoice.TotalCgst = totalCgst;
        invoice.TotalSgst = totalSgst;
        invoice.TotalIgst = totalIgst;
        invoice.GstAmount = totalCgst + totalSgst + totalIgst;
        invoice.SubTotal = subTotal;
        invoice.DiscountAmount = totalDiscount;
        invoice.NetAmount = subTotal - totalDiscount + invoice.GstAmount;

        return (auditLogs, stockDeductions);
    }
}
