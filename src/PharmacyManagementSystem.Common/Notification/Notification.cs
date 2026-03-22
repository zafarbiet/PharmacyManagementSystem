namespace PharmacyManagementSystem.Common.Notification;

public class Notification : BaseObject
{
    public Guid Id { get; set; }
    public string? NotificationType { get; set; }
    public string? Channel { get; set; }
    public string? RecipientType { get; set; }
    public Guid RecipientId { get; set; }
    public string? RecipientContact { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public Guid? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }
    public string? Status { get; set; }
    public string? FailureReason { get; set; }
    public int RetryCount { get; set; }
}
