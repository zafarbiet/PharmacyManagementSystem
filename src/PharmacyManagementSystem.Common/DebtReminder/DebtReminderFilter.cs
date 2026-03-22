namespace PharmacyManagementSystem.Common.DebtReminder;

public class DebtReminderFilter : FilterBase
{
    public Guid DebtRecordId { get; set; }
    public string? Channel { get; set; }
}
