namespace PharmacyManagementSystem.Common.DailyDiaryEntry;

public class DailyDiaryEntry : BaseObject
{
    public Guid Id { get; set; }
    public DateTimeOffset EntryDate { get; set; }
    public string? Category { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public Guid? DrugId { get; set; }
    public Guid? VendorId { get; set; }
    public Guid? PatientId { get; set; }
    public Guid? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
    public string? Priority { get; set; }
    public string? CreatedBy { get; set; }
}
