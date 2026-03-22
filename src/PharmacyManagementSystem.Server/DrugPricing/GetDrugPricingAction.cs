using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugPricing;

namespace PharmacyManagementSystem.Server.DrugPricing;

public class GetDrugPricingAction(ILogger<GetDrugPricingAction> logger, IDrugPricingRepository repository) : IGetDrugPricingAction
{
    private readonly ILogger<GetDrugPricingAction> _logger = logger;
    private readonly IDrugPricingRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DrugPricing.DrugPricing>?> GetByFilterCriteriaAsync(DrugPricingFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting drug pricings by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} drug pricings.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugPricing.DrugPricing?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting drug pricing by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved drug pricing with id: {Id}.", id);

        return result;
    }
}
