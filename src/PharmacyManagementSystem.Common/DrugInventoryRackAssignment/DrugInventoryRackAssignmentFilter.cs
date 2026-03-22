namespace PharmacyManagementSystem.Common.DrugInventoryRackAssignment;

public class DrugInventoryRackAssignmentFilter : FilterBase
{
    public Guid DrugInventoryId { get; set; }
    public Guid RackId { get; set; }
}
