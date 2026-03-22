using PharmacyManagementSystem.Common.DrugInventory;

namespace PharmacyManagementSystem.Server.DrugInventory;

public interface IDrugInventoryStorageClient
{
    Task<IReadOnlyCollection<Common.DrugInventory.DrugInventory>?> GetByFilterCriteriaAsync(DrugInventoryFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugInventory.DrugInventory?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DrugInventory.DrugInventory?> AddAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken);
    Task<Common.DrugInventory.DrugInventory?> UpdateAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
