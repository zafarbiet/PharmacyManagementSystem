namespace PharmacyManagementSystem.Common.DebtReminder;

public class DebtReminder : BaseObject
{
    public Guid Id { get; set; }
    public Guid DebtRecordId { get; set; }
    public DateTimeOffset SentAt { get; set; }
    public string? Channel { get; set; }
    public string? SentBy { get; set; }
    public string? Message { get; set; }
}
