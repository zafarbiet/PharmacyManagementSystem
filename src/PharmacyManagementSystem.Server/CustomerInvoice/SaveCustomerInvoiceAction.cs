using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.AuditLog;
using PharmacyManagementSystem.Server.Drug;
using PharmacyManagementSystem.Server.GstCalculation;

namespace PharmacyManagementSystem.Server.CustomerInvoice;

public class SaveCustomerInvoiceAction(
    ILogger<SaveCustomerInvoiceAction> logger,
    ICustomerInvoiceRepository repository,
    IDrugRepository drugRepository,
    IGstCalculationService gstCalculationService,
    ISaveAuditLogAction auditLogAction) : ISaveCustomerInvoiceAction
{
    private static readonly HashSet<string> ControlledSchedules =
        new(StringComparer.OrdinalIgnoreCase) { "H", "H1", "X" };

    private readonly ILogger<SaveCustomerInvoiceAction> _logger = logger;
    private readonly ICustomerInvoiceRepository _repository = repository;
    private readonly IDrugRepository _drugRepository = drugRepository;
    private readonly IGstCalculationService _gstCalculationService = gstCalculationService;
    private readonly ISaveAuditLogAction _auditLogAction = auditLogAction;

    public async Task<Common.CustomerInvoice.CustomerInvoice?> AddAsync(
        Common.CustomerInvoice.CustomerInvoice? customerInvoice,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        if (string.IsNullOrWhiteSpace(customerInvoice.Status))
            throw new BadRequestException("CustomerInvoice Status is required.");

        List<Common.AuditLog.AuditLog> pendingAuditLogs = [];
        if (customerInvoice.Items.Count > 0)
        {
            pendingAuditLogs = await ValidateAndComputeItemsAsync(customerInvoice, cancellationToken).ConfigureAwait(false);
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

    private async Task<List<Common.AuditLog.AuditLog>> ValidateAndComputeItemsAsync(
        Common.CustomerInvoice.CustomerInvoice invoice,
        CancellationToken cancellationToken)
    {
        var totalCgst = 0m;
        var totalSgst = 0m;
        var totalIgst = 0m;
        var subTotal = 0m;
        var totalDiscount = 0m;
        var auditLogs = new List<Common.AuditLog.AuditLog>();

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
        }

        invoice.TotalCgst = totalCgst;
        invoice.TotalSgst = totalSgst;
        invoice.TotalIgst = totalIgst;
        invoice.GstAmount = totalCgst + totalSgst + totalIgst;
        invoice.SubTotal = subTotal;
        invoice.DiscountAmount = totalDiscount;
        invoice.NetAmount = subTotal - totalDiscount + invoice.GstAmount;

        return auditLogs;
    }
}
