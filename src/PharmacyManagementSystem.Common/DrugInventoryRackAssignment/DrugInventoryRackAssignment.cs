namespace PharmacyManagementSystem.Common.DrugInventoryRackAssignment;

public class DrugInventoryRackAssignment : BaseObject
{
    public Guid Id { get; set; }
    public Guid DrugInventoryId { get; set; }
    public Guid RackId { get; set; }
    public int QuantityPlaced { get; set; }
    public DateTimeOffset PlacedAt { get; set; }
}
