using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PaymentLedger;

namespace PharmacyManagementSystem.Server.PaymentLedger;

public class PaymentLedgerRepository(ILogger<PaymentLedgerRepository> logger, IPaymentLedgerStorageClient storageClient) : IPaymentLedgerRepository
{
    private readonly ILogger<PaymentLedgerRepository> _logger = logger;
    private readonly IPaymentLedgerStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.PaymentLedger.PaymentLedger>?> GetByFilterCriteriaAsync(PaymentLedgerFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting payment ledgers by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} payment ledgers.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting payment ledger by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved payment ledger with id: {Id}.", id);

        return result;
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> AddAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);

        _logger.LogDebug("Repository: Adding payment ledger for vendor: {VendorId}.", paymentLedger.VendorId);

        var result = await _storageClient.AddAsync(paymentLedger, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added payment ledger for vendor: {VendorId}.", paymentLedger.VendorId);

        return result;
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> UpdateAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);

        _logger.LogDebug("Repository: Updating payment ledger with id: {Id}.", paymentLedger.Id);

        var result = await _storageClient.UpdateAsync(paymentLedger, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated payment ledger with id: {Id}.", paymentLedger.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing payment ledger with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed payment ledger with id: {Id}.", id);
    }
}
