using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DamageRecord;

namespace PharmacyManagementSystem.Server.DamageRecord;

public class DamageRecordRepository(ILogger<DamageRecordRepository> logger, IDamageRecordStorageClient storageClient) : IDamageRecordRepository
{
    private readonly ILogger<DamageRecordRepository> _logger = logger;
    private readonly IDamageRecordStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DamageRecord.DamageRecord>?> GetByFilterCriteriaAsync(DamageRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting damage records by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} damage records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DamageRecord.DamageRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting damage record by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved damage record with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DamageRecord.DamageRecord?> AddAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);

        _logger.LogDebug("Repository: Adding damage record.");

        var result = await _storageClient.AddAsync(damageRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added damage record.");

        return result;
    }

    public async Task<Common.DamageRecord.DamageRecord?> UpdateAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);

        _logger.LogDebug("Repository: Updating damage record with id: {Id}.", damageRecord.Id);

        var result = await _storageClient.UpdateAsync(damageRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated damage record with id: {Id}.", damageRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing damage record with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed damage record with id: {Id}.", id);
    }
}
