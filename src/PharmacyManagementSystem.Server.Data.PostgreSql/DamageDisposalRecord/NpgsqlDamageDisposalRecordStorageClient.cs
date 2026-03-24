using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DamageDisposalRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DamageDisposalRecord;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DamageDisposalRecord;

public class NpgsqlDamageDisposalRecordStorageClient(ILogger<NpgsqlDamageDisposalRecordStorageClient> logger, INpgsqlDbClient dbClient) : IDamageDisposalRecordStorageClient
{
    private readonly ILogger<NpgsqlDamageDisposalRecordStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DamageDisposalRecord.DamageDisposalRecord?>?> GetByFilterCriteriaAsync(DamageDisposalRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting damageDisposalRecords by filter criteria.");
        var sql = await DamageDisposalRecordDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageDisposalRecord.DamageDisposalRecord>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} damageDisposalRecords.", list.Count);
        return list;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting damageDisposalRecord by id: {Id}.", id);
        var sql = await DamageDisposalRecordDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageDisposalRecord.DamageDisposalRecord>(sql, cancellationToken).ConfigureAwait(false);
        var damageDisposalRecord = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved damageDisposalRecord with id: {Id}.", id);
        return damageDisposalRecord;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> AddAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);
        _logger.LogDebug("StorageClient: Adding damageDisposalRecord.");
        var sql = await DamageDisposalRecordDatabaseCommandText.GetInsertSql(damageDisposalRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageDisposalRecord.DamageDisposalRecord>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added damageDisposalRecord.");
        return inserted;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> UpdateAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(damageDisposalRecord);
        _logger.LogDebug("StorageClient: Updating damageDisposalRecord with id: {Id}.", damageDisposalRecord.Id);
        var sql = await DamageDisposalRecordDatabaseCommandText.GetUpdateSql(damageDisposalRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DamageDisposalRecord.DamageDisposalRecord>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated damageDisposalRecord with id: {Id}.", damageDisposalRecord.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing damageDisposalRecord with id: {Id}.", id);
        var sql = await DamageDisposalRecordDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed damageDisposalRecord with id: {Id}.", id);
    }
}
