using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DebtRecord;

namespace PharmacyManagementSystem.Server.DebtRecord;

public class DebtRecordRepository(ILogger<DebtRecordRepository> logger, IDebtRecordStorageClient storageClient) : IDebtRecordRepository
{
    private readonly ILogger<DebtRecordRepository> _logger = logger;
    private readonly IDebtRecordStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DebtRecord.DebtRecord>?> GetByFilterCriteriaAsync(DebtRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting debt records by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} debt records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DebtRecord.DebtRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting debt record by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved debt record with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DebtRecord.DebtRecord?> AddAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtRecord);

        _logger.LogDebug("Repository: Adding debt record for patient id: {PatientId}.", debtRecord.PatientId);

        var result = await _storageClient.AddAsync(debtRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added debt record for patient id: {PatientId}.", debtRecord.PatientId);

        return result;
    }

    public async Task<Common.DebtRecord.DebtRecord?> UpdateAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtRecord);

        _logger.LogDebug("Repository: Updating debt record with id: {Id}.", debtRecord.Id);

        var result = await _storageClient.UpdateAsync(debtRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated debt record with id: {Id}.", debtRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing debt record with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed debt record with id: {Id}.", id);
    }
}
