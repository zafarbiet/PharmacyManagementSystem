using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;

namespace PharmacyManagementSystem.Server.DrugInventoryRackAssignment;

public interface IGetDrugInventoryRackAssignmentAction
{
    Task<IReadOnlyCollection<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>?> GetByFilterCriteriaAsync(DrugInventoryRackAssignmentFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
