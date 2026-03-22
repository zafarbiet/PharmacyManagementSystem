using PharmacyManagementSystem.Common.DrugPricing;

namespace PharmacyManagementSystem.Server.DrugPricing;

public interface IDrugPricingRepository
{
    Task<IReadOnlyCollection<Common.DrugPricing.DrugPricing>?> GetByFilterCriteriaAsync(DrugPricingFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugPricing.DrugPricing?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DrugPricing.DrugPricing?> AddAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken);
    Task<Common.DrugPricing.DrugPricing?> UpdateAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
