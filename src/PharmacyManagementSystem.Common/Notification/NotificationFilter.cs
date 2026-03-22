namespace PharmacyManagementSystem.Common.Notification;

public class NotificationFilter : FilterBase
{
    public string? NotificationType { get; set; }
    public string? Channel { get; set; }
    public string? Status { get; set; }
    public Guid RecipientId { get; set; }
}
