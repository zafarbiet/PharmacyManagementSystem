using PharmacyManagementSystem.Common.DrugPricing;

namespace PharmacyManagementSystem.Server.DrugPricing;

public interface IGetDrugPricingAction
{
    Task<IReadOnlyCollection<Common.DrugPricing.DrugPricing>?> GetByFilterCriteriaAsync(DrugPricingFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugPricing.DrugPricing?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
