using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DrugPricing;

public class SaveDrugPricingAction(ILogger<SaveDrugPricingAction> logger, IDrugPricingRepository repository) : ISaveDrugPricingAction
{
    private readonly ILogger<SaveDrugPricingAction> _logger = logger;
    private readonly IDrugPricingRepository _repository = repository;

    public async Task<Common.DrugPricing.DrugPricing?> AddAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);

        if (drugPricing.DrugId == Guid.Empty)
            throw new BadRequestException("DrugPricing DrugId is required.");

        if (drugPricing.CostPrice <= 0)
            throw new BadRequestException("DrugPricing CostPrice must be > 0.");

        if (drugPricing.SellingPrice <= 0)
            throw new BadRequestException("DrugPricing SellingPrice must be > 0.");

        drugPricing.UpdatedBy = "system";

        _logger.LogDebug("Adding new drug pricing for DrugId: {DrugId}.", drugPricing.DrugId);

        var result = await _repository.AddAsync(drugPricing, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added drug pricing for DrugId: {DrugId}.", drugPricing.DrugId);

        return result;
    }

    public async Task<Common.DrugPricing.DrugPricing?> UpdateAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);

        if (drugPricing.DrugId == Guid.Empty)
            throw new BadRequestException("DrugPricing DrugId is required.");

        if (drugPricing.CostPrice <= 0)
            throw new BadRequestException("DrugPricing CostPrice must be > 0.");

        if (drugPricing.SellingPrice <= 0)
            throw new BadRequestException("DrugPricing SellingPrice must be > 0.");

        drugPricing.UpdatedBy = "system";

        _logger.LogDebug("Updating drug pricing with id: {Id}.", drugPricing.Id);

        var result = await _repository.UpdateAsync(drugPricing, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated drug pricing with id: {Id}.", drugPricing.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing drug pricing with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed drug pricing with id: {Id}.", id);
    }
}
