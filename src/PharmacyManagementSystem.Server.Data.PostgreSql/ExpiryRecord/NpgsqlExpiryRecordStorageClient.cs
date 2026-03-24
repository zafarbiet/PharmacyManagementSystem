using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.ExpiryRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.ExpiryRecord;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.ExpiryRecord;

public class NpgsqlExpiryRecordStorageClient(ILogger<NpgsqlExpiryRecordStorageClient> logger, INpgsqlDbClient dbClient) : IExpiryRecordStorageClient
{
    private readonly ILogger<NpgsqlExpiryRecordStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.ExpiryRecord.ExpiryRecord?>?> GetByFilterCriteriaAsync(ExpiryRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting expiryRecords by filter criteria.");
        var sql = await ExpiryRecordDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryRecord.ExpiryRecord>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} expiryRecords.", list.Count);
        return list;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting expiryRecord by id: {Id}.", id);
        var sql = await ExpiryRecordDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryRecord.ExpiryRecord>(sql, cancellationToken).ConfigureAwait(false);
        var expiryRecord = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved expiryRecord with id: {Id}.", id);
        return expiryRecord;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> AddAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);
        _logger.LogDebug("StorageClient: Adding expiryRecord.");
        var sql = await ExpiryRecordDatabaseCommandText.GetInsertSql(expiryRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryRecord.ExpiryRecord>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added expiryRecord.");
        return inserted;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> UpdateAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);
        _logger.LogDebug("StorageClient: Updating expiryRecord with id: {Id}.", expiryRecord.Id);
        var sql = await ExpiryRecordDatabaseCommandText.GetUpdateSql(expiryRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.ExpiryRecord.ExpiryRecord>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated expiryRecord with id: {Id}.", expiryRecord.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing expiryRecord with id: {Id}.", id);
        var sql = await ExpiryRecordDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed expiryRecord with id: {Id}.", id);
    }
}
