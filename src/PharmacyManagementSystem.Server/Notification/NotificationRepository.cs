using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Notification;

namespace PharmacyManagementSystem.Server.Notification;

public class NotificationRepository(ILogger<NotificationRepository> logger, INotificationStorageClient storageClient) : INotificationRepository
{
    private readonly ILogger<NotificationRepository> _logger = logger;
    private readonly INotificationStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Notification.Notification>?> GetByFilterCriteriaAsync(NotificationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting notifications by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} notifications.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Notification.Notification?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting notification by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved notification with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Notification.Notification?> AddAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        _logger.LogDebug("Repository: Adding notification.");

        var result = await _storageClient.AddAsync(notification, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added notification.");

        return result;
    }

    public async Task<Common.Notification.Notification?> UpdateAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        _logger.LogDebug("Repository: Updating notification with id: {Id}.", notification.Id);

        var result = await _storageClient.UpdateAsync(notification, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated notification with id: {Id}.", notification.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing notification with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed notification with id: {Id}.", id);
    }
}
