namespace PharmacyManagementSystem.Common.DrugInventory;

public class DrugInventoryFilter : FilterBase
{
    public Guid? DrugId { get; set; }
    public string? BatchNumber { get; set; }
    public DateTimeOffset? ExpiresBeforeDate { get; set; }
}
