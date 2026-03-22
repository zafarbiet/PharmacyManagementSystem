using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.ExpiryRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.ExpiryRecord;

namespace PharmacyManagementSystem.Server.Data.SqlServer.ExpiryRecord;

public class SqlServerExpiryRecordStorageClient(ILogger<SqlServerExpiryRecordStorageClient> logger, ISqlServerDbClient dbClient) : IExpiryRecordStorageClient
{
    private readonly ILogger<SqlServerExpiryRecordStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.ExpiryRecord.ExpiryRecord>?> GetByFilterCriteriaAsync(ExpiryRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting expiry records by filter criteria.");

        var sql = await ExpiryRecordDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryRecord.ExpiryRecord>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} expiry records.", list.Count);

        return list;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting expiry record by id: {Id}.", id);

        var sql = await ExpiryRecordDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryRecord.ExpiryRecord>(sql, cancellationToken).ConfigureAwait(false);

        var item = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved expiry record with id: {Id}.", id);

        return item;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> AddAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);

        _logger.LogDebug("StorageClient: Adding expiry record for drug inventory: {DrugInventoryId}.", expiryRecord.DrugInventoryId);

        var sql = await ExpiryRecordDatabaseCommandText.GetInsertSql(expiryRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryRecord.ExpiryRecord>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added expiry record for drug inventory: {DrugInventoryId}.", expiryRecord.DrugInventoryId);

        return inserted;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> UpdateAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);

        _logger.LogDebug("StorageClient: Updating expiry record with id: {Id}.", expiryRecord.Id);

        var sql = await ExpiryRecordDatabaseCommandText.GetUpdateSql(expiryRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryRecord.ExpiryRecord>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated expiry record with id: {Id}.", expiryRecord.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing expiry record with id: {Id}.", id);

        var sql = await ExpiryRecordDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed expiry record with id: {Id}.", id);
    }
}
