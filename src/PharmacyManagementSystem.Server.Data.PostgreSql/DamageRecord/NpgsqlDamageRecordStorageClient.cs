using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DamageRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DamageRecord;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DamageRecord;

public class NpgsqlDamageRecordStorageClient(ILogger<NpgsqlDamageRecordStorageClient> logger, INpgsqlDbClient dbClient) : IDamageRecordStorageClient
{
    private readonly ILogger<NpgsqlDamageRecordStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DamageRecord.DamageRecord?>?> GetByFilterCriteriaAsync(DamageRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting damageRecords by filter criteria.");
        var sql = await DamageRecordDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageRecord.DamageRecord>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} damageRecords.", list.Count);
        return list;
    }

    public async Task<Common.DamageRecord.DamageRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting damageRecord by id: {Id}.", id);
        var sql = await DamageRecordDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageRecord.DamageRecord>(sql, cancellationToken).ConfigureAwait(false);
        var damageRecord = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved damageRecord with id: {Id}.", id);
        return damageRecord;
    }

    public async Task<Common.DamageRecord.DamageRecord?> AddAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);
        _logger.LogDebug("StorageClient: Adding damageRecord.");
        var sql = await DamageRecordDatabaseCommandText.GetInsertSql(damageRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageRecord.DamageRecord>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added damageRecord.");
        return inserted;
    }

    public async Task<Common.DamageRecord.DamageRecord?> UpdateAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);
        _logger.LogDebug("StorageClient: Updating damageRecord with id: {Id}.", damageRecord.Id);
        var sql = await DamageRecordDatabaseCommandText.GetUpdateSql(damageRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageRecord.DamageRecord>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated damageRecord with id: {Id}.", damageRecord.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing damageRecord with id: {Id}.", id);
        var sql = await DamageRecordDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed damageRecord with id: {Id}.", id);
    }
}
