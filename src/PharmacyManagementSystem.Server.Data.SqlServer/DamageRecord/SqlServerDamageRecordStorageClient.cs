using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DamageRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.DamageRecord;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DamageRecord;

public class SqlServerDamageRecordStorageClient(ILogger<SqlServerDamageRecordStorageClient> logger, ISqlServerDbClient dbClient) : IDamageRecordStorageClient
{
    private readonly ILogger<SqlServerDamageRecordStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DamageRecord.DamageRecord>?> GetByFilterCriteriaAsync(DamageRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting damage records by filter criteria.");

        var sql = await DamageRecordDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageRecord.DamageRecord>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} damage records.", list.Count);

        return list;
    }

    public async Task<Common.DamageRecord.DamageRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting damage record by id: {Id}.", id);

        var sql = await DamageRecordDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageRecord.DamageRecord>(sql, cancellationToken).ConfigureAwait(false);

        var damageRecord = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved damage record with id: {Id}.", id);

        return damageRecord;
    }

    public async Task<Common.DamageRecord.DamageRecord?> AddAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);

        _logger.LogDebug("StorageClient: Adding damage record.");

        var sql = await DamageRecordDatabaseCommandText.GetInsertSql(damageRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageRecord.DamageRecord>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added damage record.");

        return inserted;
    }

    public async Task<Common.DamageRecord.DamageRecord?> UpdateAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageRecord);

        _logger.LogDebug("StorageClient: Updating damage record with id: {Id}.", damageRecord.Id);

        var sql = await DamageRecordDatabaseCommandText.GetUpdateSql(damageRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageRecord.DamageRecord>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated damage record with id: {Id}.", damageRecord.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing damage record with id: {Id}.", id);

        var sql = await DamageRecordDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed damage record with id: {Id}.", id);
    }
}
