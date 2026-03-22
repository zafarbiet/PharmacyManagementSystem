using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DamageDisposalRecord;

namespace PharmacyManagementSystem.Server.DamageDisposalRecord;

public class DamageDisposalRecordRepository(ILogger<DamageDisposalRecordRepository> logger, IDamageDisposalRecordStorageClient storageClient) : IDamageDisposalRecordRepository
{
    private readonly ILogger<DamageDisposalRecordRepository> _logger = logger;
    private readonly IDamageDisposalRecordStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DamageDisposalRecord.DamageDisposalRecord>?> GetByFilterCriteriaAsync(DamageDisposalRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting damage disposal records by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} damage disposal records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting damage disposal record by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved damage disposal record with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> AddAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);

        _logger.LogDebug("Repository: Adding damage disposal record.");

        var result = await _storageClient.AddAsync(damageDisposalRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added damage disposal record.");

        return result;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> UpdateAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);

        _logger.LogDebug("Repository: Updating damage disposal record with id: {Id}.", damageDisposalRecord.Id);

        var result = await _storageClient.UpdateAsync(damageDisposalRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated damage disposal record with id: {Id}.", damageDisposalRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing damage disposal record with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed damage disposal record with id: {Id}.", id);
    }
}
