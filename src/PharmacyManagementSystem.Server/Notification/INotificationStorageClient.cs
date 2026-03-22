using PharmacyManagementSystem.Common.Notification;

namespace PharmacyManagementSystem.Server.Notification;

public interface INotificationStorageClient
{
    Task<IReadOnlyCollection<Common.Notification.Notification>?> GetByFilterCriteriaAsync(NotificationFilter filter, CancellationToken cancellationToken);
    Task<Common.Notification.Notification?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Notification.Notification?> AddAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken);
    Task<Common.Notification.Notification?> UpdateAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
