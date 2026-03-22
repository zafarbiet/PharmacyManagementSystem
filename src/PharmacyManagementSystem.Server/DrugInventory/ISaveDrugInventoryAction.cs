namespace PharmacyManagementSystem.Server.DrugInventory;

public interface ISaveDrugInventoryAction
{
    Task<Common.DrugInventory.DrugInventory?> AddAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken);
    Task<Common.DrugInventory.DrugInventory?> UpdateAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
