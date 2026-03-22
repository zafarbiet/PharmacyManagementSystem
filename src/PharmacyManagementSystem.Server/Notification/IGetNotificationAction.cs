using PharmacyManagementSystem.Common.Notification;

namespace PharmacyManagementSystem.Server.Notification;

public interface IGetNotificationAction
{
    Task<IReadOnlyCollection<Common.Notification.Notification>?> GetByFilterCriteriaAsync(NotificationFilter filter, CancellationToken cancellationToken);
    Task<Common.Notification.Notification?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
