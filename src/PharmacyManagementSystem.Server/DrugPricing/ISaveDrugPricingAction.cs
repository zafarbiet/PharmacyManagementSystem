namespace PharmacyManagementSystem.Server.DrugPricing;

public interface ISaveDrugPricingAction
{
    Task<Common.DrugPricing.DrugPricing?> AddAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken);
    Task<Common.DrugPricing.DrugPricing?> UpdateAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
