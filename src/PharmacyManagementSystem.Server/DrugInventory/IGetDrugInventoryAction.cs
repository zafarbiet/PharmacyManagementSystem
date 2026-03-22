using PharmacyManagementSystem.Common.DrugInventory;

namespace PharmacyManagementSystem.Server.DrugInventory;

public interface IGetDrugInventoryAction
{
    Task<IReadOnlyCollection<Common.DrugInventory.DrugInventory>?> GetByFilterCriteriaAsync(DrugInventoryFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugInventory.DrugInventory?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
