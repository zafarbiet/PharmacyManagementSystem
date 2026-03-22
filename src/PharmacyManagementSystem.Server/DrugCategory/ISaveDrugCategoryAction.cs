namespace PharmacyManagementSystem.Server.DrugCategory;

public interface ISaveDrugCategoryAction
{
    Task<Common.DrugCategory.DrugCategory?> AddAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken);
    Task<Common.DrugCategory.DrugCategory?> UpdateAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
