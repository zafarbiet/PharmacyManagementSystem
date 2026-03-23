using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Vendor;

namespace PharmacyManagementSystem.Server.PaymentLedger;

public class SavePaymentLedgerAction(
    ILogger<SavePaymentLedgerAction> logger,
    IPaymentLedgerRepository repository,
    IVendorRepository vendorRepository) : ISavePaymentLedgerAction
{
    private readonly ILogger<SavePaymentLedgerAction> _logger = logger;
    private readonly IPaymentLedgerRepository _repository = repository;
    private readonly IVendorRepository _vendorRepository = vendorRepository;

    public async Task<Common.PaymentLedger.PaymentLedger?> AddAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);

        if (paymentLedger.VendorId == Guid.Empty)
            throw new BadRequestException("Payment ledger VendorId is required.");

        if (paymentLedger.InvoicedAmount <= 0)
            throw new BadRequestException("Payment ledger InvoicedAmount must be greater than zero.");

        paymentLedger.UpdatedBy = "system";

        _logger.LogDebug("Adding new payment ledger for vendor: {VendorId}.", paymentLedger.VendorId);

        var result = await _repository.AddAsync(paymentLedger, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added payment ledger for vendor: {VendorId}.", paymentLedger.VendorId);

        return result;
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> UpdateAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);

        if (paymentLedger.VendorId == Guid.Empty)
            throw new BadRequestException("Payment ledger VendorId is required.");

        paymentLedger.UpdatedBy = "system";

        _logger.LogDebug("Updating payment ledger with id: {Id}.", paymentLedger.Id);

        var result = await _repository.UpdateAsync(paymentLedger, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated payment ledger with id: {Id}.", paymentLedger.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing payment ledger with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed payment ledger with id: {Id}.", id);
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> RecordPaymentAsync(Guid ledgerId, decimal paymentAmount, CancellationToken cancellationToken)
    {
        if (paymentAmount <= 0)
            throw new BadRequestException("Payment amount must be greater than zero.");

        _logger.LogDebug("Recording payment of {Amount} for ledger {LedgerId}.", paymentAmount, ledgerId);

        var ledger = await _repository.GetByIdAsync(ledgerId.ToString(), cancellationToken).ConfigureAwait(false);
        if (ledger is null)
            throw new BadRequestException($"Payment ledger {ledgerId} not found.");

        if (ledger.Status is "Paid")
            throw new ConflictException($"Payment ledger {ledgerId} is already fully paid.");

        ledger.PaidAmount += paymentAmount;
        ledger.Status = ledger.PaidAmount >= ledger.InvoicedAmount ? "Paid" : "PartiallyPaid";
        ledger.UpdatedBy = "system";

        var result = await _repository.UpdateAsync(ledger, cancellationToken).ConfigureAwait(false);

        // Update vendor outstanding balance
        var vendor = await _vendorRepository.GetByIdAsync(ledger.VendorId.ToString(), cancellationToken).ConfigureAwait(false);
        if (vendor is not null)
        {
            vendor.OutstandingBalance = Math.Max(0, vendor.OutstandingBalance - paymentAmount);
            vendor.UpdatedBy = "system";
            await _vendorRepository.UpdateAsync(vendor, cancellationToken).ConfigureAwait(false);
        }

        _logger.LogDebug("Recorded payment for ledger {LedgerId}. New status: {Status}.", ledgerId, ledger.Status);

        return result;
    }
}
