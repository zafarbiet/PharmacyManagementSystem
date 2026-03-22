using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DebtPayment;

namespace PharmacyManagementSystem.Server.DebtPayment;

public class DebtPaymentRepository(ILogger<DebtPaymentRepository> logger, IDebtPaymentStorageClient storageClient) : IDebtPaymentRepository
{
    private readonly ILogger<DebtPaymentRepository> _logger = logger;
    private readonly IDebtPaymentStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DebtPayment.DebtPayment>?> GetByFilterCriteriaAsync(DebtPaymentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting debt payments by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} debt payments.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DebtPayment.DebtPayment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting debt payment by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved debt payment with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DebtPayment.DebtPayment?> AddAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtPayment);

        _logger.LogDebug("Repository: Adding debt payment for debt record id: {DebtRecordId}.", debtPayment.DebtRecordId);

        var result = await _storageClient.AddAsync(debtPayment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added debt payment for debt record id: {DebtRecordId}.", debtPayment.DebtRecordId);

        return result;
    }

    public async Task<Common.DebtPayment.DebtPayment?> UpdateAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtPayment);

        _logger.LogDebug("Repository: Updating debt payment with id: {Id}.", debtPayment.Id);

        var result = await _storageClient.UpdateAsync(debtPayment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated debt payment with id: {Id}.", debtPayment.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing debt payment with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed debt payment with id: {Id}.", id);
    }
}
