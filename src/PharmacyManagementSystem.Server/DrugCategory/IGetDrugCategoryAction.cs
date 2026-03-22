using PharmacyManagementSystem.Common.DrugCategory;

namespace PharmacyManagementSystem.Server.DrugCategory;

public interface IGetDrugCategoryAction
{
    Task<IReadOnlyCollection<Common.DrugCategory.DrugCategory>?> GetByFilterCriteriaAsync(DrugCategoryFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugCategory.DrugCategory?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
