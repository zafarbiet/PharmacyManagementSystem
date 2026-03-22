using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DailyDiaryEntry;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.DailyDiaryEntry;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DailyDiaryEntry;

public class SqlServerDailyDiaryEntryStorageClient(ILogger<SqlServerDailyDiaryEntryStorageClient> logger, ISqlServerDbClient dbClient) : IDailyDiaryEntryStorageClient
{
    private readonly ILogger<SqlServerDailyDiaryEntryStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DailyDiaryEntry.DailyDiaryEntry>?> GetByFilterCriteriaAsync(DailyDiaryEntryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting daily diary entries by filter criteria.");

        var sql = await DailyDiaryEntryDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DailyDiaryEntry.DailyDiaryEntry>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} daily diary entries.", list.Count);

        return list;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting daily diary entry by id: {Id}.", id);

        var sql = await DailyDiaryEntryDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DailyDiaryEntry.DailyDiaryEntry>(sql, cancellationToken).ConfigureAwait(false);

        var dailyDiaryEntry = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved daily diary entry with id: {Id}.", id);

        return dailyDiaryEntry;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> AddAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);

        _logger.LogDebug("StorageClient: Adding daily diary entry.");

        var sql = await DailyDiaryEntryDatabaseCommandText.GetInsertSql(dailyDiaryEntry).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DailyDiaryEntry.DailyDiaryEntry>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added daily diary entry.");

        return inserted;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> UpdateAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);

        _logger.LogDebug("StorageClient: Updating daily diary entry with id: {Id}.", dailyDiaryEntry.Id);

        var sql = await DailyDiaryEntryDatabaseCommandText.GetUpdateSql(dailyDiaryEntry).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DailyDiaryEntry.DailyDiaryEntry>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated daily diary entry with id: {Id}.", dailyDiaryEntry.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing daily diary entry with id: {Id}.", id);

        var sql = await DailyDiaryEntryDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed daily diary entry with id: {Id}.", id);
    }
}
