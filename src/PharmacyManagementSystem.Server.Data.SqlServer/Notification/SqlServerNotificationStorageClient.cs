using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Notification;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Notification;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Notification;

public class SqlServerNotificationStorageClient(ILogger<SqlServerNotificationStorageClient> logger, ISqlServerDbClient dbClient) : INotificationStorageClient
{
    private readonly ILogger<SqlServerNotificationStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Notification.Notification>?> GetByFilterCriteriaAsync(NotificationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting notifications by filter criteria.");

        var sql = await NotificationDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Notification.Notification>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} notifications.", list.Count);

        return list;
    }

    public async Task<Common.Notification.Notification?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting notification by id: {Id}.", id);

        var sql = await NotificationDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Notification.Notification>(sql, cancellationToken).ConfigureAwait(false);

        var notification = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved notification with id: {Id}.", id);

        return notification;
    }

    public async Task<Common.Notification.Notification?> AddAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        _logger.LogDebug("StorageClient: Adding notification.");

        var sql = await NotificationDatabaseCommandText.GetInsertSql(notification).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Notification.Notification>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added notification.");

        return inserted;
    }

    public async Task<Common.Notification.Notification?> UpdateAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        _logger.LogDebug("StorageClient: Updating notification with id: {Id}.", notification.Id);

        var sql = await NotificationDatabaseCommandText.GetUpdateSql(notification).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Notification.Notification>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated notification with id: {Id}.", notification.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing notification with id: {Id}.", id);

        var sql = await NotificationDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed notification with id: {Id}.", id);
    }
}
