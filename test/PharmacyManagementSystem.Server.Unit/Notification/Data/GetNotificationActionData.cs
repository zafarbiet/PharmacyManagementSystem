using PharmacyManagementSystem.Common.Notification;

namespace PharmacyManagementSystem.Server.Unit.Notification.Data;

public static class GetNotificationActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new NotificationFilter { Status = "Pending" },
            new List<Common.Notification.Notification>
            {
                new() { Id = Guid.NewGuid(), NotificationType = "ExpiryAlert", Channel = "Email", Status = "Pending", RecipientType = "User", IsActive = true }
            }
        };

        yield return new object[]
        {
            new NotificationFilter(),
            new List<Common.Notification.Notification>
            {
                new() { Id = Guid.NewGuid(), NotificationType = "ExpiryAlert", Channel = "Email", Status = "Pending", RecipientType = "User", IsActive = true },
                new() { Id = Guid.NewGuid(), NotificationType = "DebtReminder", Channel = "SMS", Status = "Sent", RecipientType = "Patient", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.Notification.Notification { Id = id, NotificationType = "ExpiryAlert", Channel = "Email", Status = "Pending", RecipientType = "User", IsActive = true }
        };
    }
}
