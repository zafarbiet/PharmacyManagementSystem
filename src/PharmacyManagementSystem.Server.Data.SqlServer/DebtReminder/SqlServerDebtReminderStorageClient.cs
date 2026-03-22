using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DebtReminder;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.DebtReminder;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DebtReminder;

public class SqlServerDebtReminderStorageClient(ILogger<SqlServerDebtReminderStorageClient> logger, ISqlServerDbClient dbClient) : IDebtReminderStorageClient
{
    private readonly ILogger<SqlServerDebtReminderStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DebtReminder.DebtReminder>?> GetByFilterCriteriaAsync(DebtReminderFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting debt reminders by filter criteria.");

        var sql = await DebtReminderDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtReminder.DebtReminder>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} debt reminders.", list.Count);

        return list;
    }

    public async Task<Common.DebtReminder.DebtReminder?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting debt reminder by id: {Id}.", id);

        var sql = await DebtReminderDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtReminder.DebtReminder>(sql, cancellationToken).ConfigureAwait(false);

        var debtReminder = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved debt reminder with id: {Id}.", id);

        return debtReminder;
    }

    public async Task<Common.DebtReminder.DebtReminder?> AddAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtReminder);

        _logger.LogDebug("StorageClient: Adding debt reminder.");

        var sql = await DebtReminderDatabaseCommandText.GetInsertSql(debtReminder).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtReminder.DebtReminder>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added debt reminder.");

        return inserted;
    }

    public async Task<Common.DebtReminder.DebtReminder?> UpdateAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtReminder);

        _logger.LogDebug("StorageClient: Updating debt reminder with id: {Id}.", debtReminder.Id);

        var sql = await DebtReminderDatabaseCommandText.GetUpdateSql(debtReminder).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtReminder.DebtReminder>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated debt reminder with id: {Id}.", debtReminder.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing debt reminder with id: {Id}.", id);

        var sql = await DebtReminderDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed debt reminder with id: {Id}.", id);
    }
}
