using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DisposalRecord;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.DisposalRecord;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DisposalRecord;

public class SqlServerDisposalRecordStorageClient(ILogger<SqlServerDisposalRecordStorageClient> logger, ISqlServerDbClient dbClient) : IDisposalRecordStorageClient
{
    private readonly ILogger<SqlServerDisposalRecordStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DisposalRecord.DisposalRecord>?> GetByFilterCriteriaAsync(DisposalRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting disposal records by filter criteria.");

        var sql = await DisposalRecordDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DisposalRecord.DisposalRecord>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} disposal records.", list.Count);

        return list;
    }

    public async Task<Common.DisposalRecord.DisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting disposal record by id: {Id}.", id);

        var sql = await DisposalRecordDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DisposalRecord.DisposalRecord>(sql, cancellationToken).ConfigureAwait(false);

        var item = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved disposal record with id: {Id}.", id);

        return item;
    }

    public async Task<Common.DisposalRecord.DisposalRecord?> AddAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(disposalRecord);

        _logger.LogDebug("StorageClient: Adding disposal record for expiry record: {ExpiryRecordId}.", disposalRecord.ExpiryRecordId);

        var sql = await DisposalRecordDatabaseCommandText.GetInsertSql(disposalRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DisposalRecord.DisposalRecord>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added disposal record for expiry record: {ExpiryRecordId}.", disposalRecord.ExpiryRecordId);

        return inserted;
    }

    public async Task<Common.DisposalRecord.DisposalRecord?> UpdateAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(disposalRecord);

        _logger.LogDebug("StorageClient: Updating disposal record with id: {Id}.", disposalRecord.Id);

        var sql = await DisposalRecordDatabaseCommandText.GetUpdateSql(disposalRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DisposalRecord.DisposalRecord>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated disposal record with id: {Id}.", disposalRecord.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing disposal record with id: {Id}.", id);

        var sql = await DisposalRecordDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed disposal record with id: {Id}.", id);
    }
}
