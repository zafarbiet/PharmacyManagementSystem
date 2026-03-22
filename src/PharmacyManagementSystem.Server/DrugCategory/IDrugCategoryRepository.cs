using PharmacyManagementSystem.Common.DrugCategory;

namespace PharmacyManagementSystem.Server.DrugCategory;

public interface IDrugCategoryRepository
{
    Task<IReadOnlyCollection<Common.DrugCategory.DrugCategory>?> GetByFilterCriteriaAsync(DrugCategoryFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugCategory.DrugCategory?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DrugCategory.DrugCategory?> AddAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken);
    Task<Common.DrugCategory.DrugCategory?> UpdateAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
