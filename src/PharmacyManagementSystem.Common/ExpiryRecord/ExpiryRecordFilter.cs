namespace PharmacyManagementSystem.Common.ExpiryRecord;

public class ExpiryRecordFilter : FilterBase
{
    public Guid? DrugInventoryId { get; set; }
    public string? Status { get; set; }
}
