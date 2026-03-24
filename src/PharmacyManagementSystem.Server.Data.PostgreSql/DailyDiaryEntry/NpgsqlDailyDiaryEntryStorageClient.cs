using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DailyDiaryEntry;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DailyDiaryEntry;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DailyDiaryEntry;

public class NpgsqlDailyDiaryEntryStorageClient(ILogger<NpgsqlDailyDiaryEntryStorageClient> logger, INpgsqlDbClient dbClient) : IDailyDiaryEntryStorageClient
{
    private readonly ILogger<NpgsqlDailyDiaryEntryStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DailyDiaryEntry.DailyDiaryEntry?>?> GetByFilterCriteriaAsync(DailyDiaryEntryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting dailyDiaryEntrys by filter criteria.");
        var sql = await DailyDiaryEntryDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DailyDiaryEntry.DailyDiaryEntry>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} dailyDiaryEntrys.", list.Count);
        return list;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting dailyDiaryEntry by id: {Id}.", id);
        var sql = await DailyDiaryEntryDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DailyDiaryEntry.DailyDiaryEntry>(sql, cancellationToken).ConfigureAwait(false);
        var dailyDiaryEntry = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved dailyDiaryEntry with id: {Id}.", id);
        return dailyDiaryEntry;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> AddAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);
        _logger.LogDebug("StorageClient: Adding dailyDiaryEntry.");
        var sql = await DailyDiaryEntryDatabaseCommandText.GetInsertSql(dailyDiaryEntry).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DailyDiaryEntry.DailyDiaryEntry>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added dailyDiaryEntry.");
        return inserted;
    }

    public async Task<Common.DailyDiaryEntry.DailyDiaryEntry?> UpdateAsync(Common.DailyDiaryEntry.DailyDiaryEntry? dailyDiaryEntry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dailyDiaryEntry);
        _logger.LogDebug("StorageClient: Updating dailyDiaryEntry with id: {Id}.", dailyDiaryEntry.Id);
        var sql = await DailyDiaryEntryDatabaseCommandText.GetUpdateSql(dailyDiaryEntry).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DailyDiaryEntry.DailyDiaryEntry>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated dailyDiaryEntry with id: {Id}.", dailyDiaryEntry.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing dailyDiaryEntry with id: {Id}.", id);
        var sql = await DailyDiaryEntryDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed dailyDiaryEntry with id: {Id}.", id);
    }
}
