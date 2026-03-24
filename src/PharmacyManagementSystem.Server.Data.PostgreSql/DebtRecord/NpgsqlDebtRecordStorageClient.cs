using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DebtRecord;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DebtRecord;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DebtRecord;

public class NpgsqlDebtRecordStorageClient(ILogger<NpgsqlDebtRecordStorageClient> logger, INpgsqlDbClient dbClient) : IDebtRecordStorageClient
{
    private readonly ILogger<NpgsqlDebtRecordStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DebtRecord.DebtRecord?>?> GetByFilterCriteriaAsync(DebtRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting debtRecords by filter criteria.");
        var sql = await DebtRecordDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtRecord.DebtRecord>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} debtRecords.", list.Count);
        return list;
    }

    public async Task<Common.DebtRecord.DebtRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting debtRecord by id: {Id}.", id);
        var sql = await DebtRecordDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtRecord.DebtRecord>(sql, cancellationToken).ConfigureAwait(false);
        var debtRecord = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved debtRecord with id: {Id}.", id);
        return debtRecord;
    }

    public async Task<Common.DebtRecord.DebtRecord?> AddAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtRecord);
        _logger.LogDebug("StorageClient: Adding debtRecord.");
        var sql = await DebtRecordDatabaseCommandText.GetInsertSql(debtRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtRecord.DebtRecord>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added debtRecord.");
        return inserted;
    }

    public async Task<Common.DebtRecord.DebtRecord?> UpdateAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtRecord);
        _logger.LogDebug("StorageClient: Updating debtRecord with id: {Id}.", debtRecord.Id);
        var sql = await DebtRecordDatabaseCommandText.GetUpdateSql(debtRecord).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtRecord.DebtRecord>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated debtRecord with id: {Id}.", debtRecord.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing debtRecord with id: {Id}.", id);
        var sql = await DebtRecordDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed debtRecord with id: {Id}.", id);
    }
}
