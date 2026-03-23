namespace PharmacyManagementSystem.Common.DebtRecord;

public class DebtRecordFilter : FilterBase
{
    public Guid? PatientId { get; set; }
    public string? Status { get; set; }
}
