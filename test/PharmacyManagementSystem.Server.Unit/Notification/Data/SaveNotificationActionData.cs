namespace PharmacyManagementSystem.Server.Unit.Notification.Data;

public static class SaveNotificationActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.Notification.Notification { NotificationType = "ExpiryAlert", Channel = "Email", Status = "Pending", RecipientType = "User" },
            new Common.Notification.Notification { Id = Guid.NewGuid(), NotificationType = "ExpiryAlert", Channel = "Email", Status = "Pending", RecipientType = "User", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.Notification.Notification { NotificationType = string.Empty, Channel = "Email", Status = "Pending", RecipientType = "User" }
        };

        yield return new object[]
        {
            new Common.Notification.Notification { NotificationType = "ExpiryAlert", Channel = string.Empty, Status = "Pending", RecipientType = "User" }
        };

        yield return new object[]
        {
            new Common.Notification.Notification { NotificationType = "ExpiryAlert", Channel = "Email", Status = string.Empty, RecipientType = "User" }
        };

        yield return new object[]
        {
            new Common.Notification.Notification { NotificationType = "ExpiryAlert", Channel = "Email", Status = "Pending", RecipientType = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Notification.Notification { Id = id, NotificationType = "DebtReminder", Channel = "SMS", Status = "Sent", RecipientType = "Patient" },
            new Common.Notification.Notification { Id = id, NotificationType = "DebtReminder", Channel = "SMS", Status = "Sent", RecipientType = "Patient", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
