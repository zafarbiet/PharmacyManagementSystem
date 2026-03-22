using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DamageDisposalRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.DamageDisposalRecord;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DamageDisposalRecord;

public class SqlServerDamageDisposalRecordStorageClient(ILogger<SqlServerDamageDisposalRecordStorageClient> logger, ISqlServerDbClient dbClient) : IDamageDisposalRecordStorageClient
{
    private readonly ILogger<SqlServerDamageDisposalRecordStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DamageDisposalRecord.DamageDisposalRecord>?> GetByFilterCriteriaAsync(DamageDisposalRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting damage disposal records by filter criteria.");

        var sql = await DamageDisposalRecordDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageDisposalRecord.DamageDisposalRecord>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} damage disposal records.", list.Count);

        return list;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting damage disposal record by id: {Id}.", id);

        var sql = await DamageDisposalRecordDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageDisposalRecord.DamageDisposalRecord>(sql, cancellationToken).ConfigureAwait(false);

        var damageDisposalRecord = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved damage disposal record with id: {Id}.", id);

        return damageDisposalRecord;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> AddAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);

        _logger.LogDebug("StorageClient: Adding damage disposal record.");

        var sql = await DamageDisposalRecordDatabaseCommandText.GetInsertSql(damageDisposalRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageDisposalRecord.DamageDisposalRecord>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added damage disposal record.");

        return inserted;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> UpdateAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);

        _logger.LogDebug("StorageClient: Updating damage disposal record with id: {Id}.", damageDisposalRecord.Id);

        var sql = await DamageDisposalRecordDatabaseCommandText.GetUpdateSql(damageDisposalRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageDisposalRecord.DamageDisposalRecord>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated damage disposal record with id: {Id}.", damageDisposalRecord.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing damage disposal record with id: {Id}.", id);

        var sql = await DamageDisposalRecordDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed damage disposal record with id: {Id}.", id);
    }
}
