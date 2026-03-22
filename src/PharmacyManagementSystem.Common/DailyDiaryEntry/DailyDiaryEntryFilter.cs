namespace PharmacyManagementSystem.Common.DailyDiaryEntry;

public class DailyDiaryEntryFilter : FilterBase
{
    public string? Category { get; set; }
    public Guid DrugId { get; set; }
    public Guid PatientId { get; set; }
    public string? Priority { get; set; }
}
