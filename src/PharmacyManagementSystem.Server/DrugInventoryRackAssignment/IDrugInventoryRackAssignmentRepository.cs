using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;

namespace PharmacyManagementSystem.Server.DrugInventoryRackAssignment;

public interface IDrugInventoryRackAssignmentRepository
{
    Task<IReadOnlyCollection<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>?> GetByFilterCriteriaAsync(DrugInventoryRackAssignmentFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> AddAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken);
    Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> UpdateAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
