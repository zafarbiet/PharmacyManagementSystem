namespace PharmacyManagementSystem.Server.Notification;

public interface ISaveNotificationAction
{
    Task<Common.Notification.Notification?> AddAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken);
    Task<Common.Notification.Notification?> UpdateAsync(Common.Notification.Notification? notification, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
