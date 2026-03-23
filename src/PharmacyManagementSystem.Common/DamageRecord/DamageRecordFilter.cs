namespace PharmacyManagementSystem.Common.DamageRecord;

public class DamageRecordFilter : FilterBase
{
    public Guid? DrugInventoryId { get; set; }
    public string? Status { get; set; }
}
