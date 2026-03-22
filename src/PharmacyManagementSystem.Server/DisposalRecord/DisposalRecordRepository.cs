using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DisposalRecord;

namespace PharmacyManagementSystem.Server.DisposalRecord;

public class DisposalRecordRepository(ILogger<DisposalRecordRepository> logger, IDisposalRecordStorageClient storageClient) : IDisposalRecordRepository
{
    private readonly ILogger<DisposalRecordRepository> _logger = logger;
    private readonly IDisposalRecordStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DisposalRecord.DisposalRecord>?> GetByFilterCriteriaAsync(DisposalRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting disposal records by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} disposal records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DisposalRecord.DisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting disposal record by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved disposal record with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DisposalRecord.DisposalRecord?> AddAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(disposalRecord);

        _logger.LogDebug("Repository: Adding disposal record for expiry record: {ExpiryRecordId}.", disposalRecord.ExpiryRecordId);

        var result = await _storageClient.AddAsync(disposalRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added disposal record for expiry record: {ExpiryRecordId}.", disposalRecord.ExpiryRecordId);

        return result;
    }

    public async Task<Common.DisposalRecord.DisposalRecord?> UpdateAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(disposalRecord);

        _logger.LogDebug("Repository: Updating disposal record with id: {Id}.", disposalRecord.Id);

        var result = await _storageClient.UpdateAsync(disposalRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated disposal record with id: {Id}.", disposalRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing disposal record with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed disposal record with id: {Id}.", id);
    }
}
