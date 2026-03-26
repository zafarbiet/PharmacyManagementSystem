using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Promotion;

namespace PharmacyManagementSystem.Server.Promotion;

public class GetPromotionAction(ILogger<GetPromotionAction> logger, IPromotionRepository repository) : IGetPromotionAction
{
    private readonly ILogger<GetPromotionAction> _logger = logger;
    private readonly IPromotionRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Promotion.Promotion>?> GetByFilterCriteriaAsync(PromotionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting promotions by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} promotions.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Promotion.Promotion?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting promotion by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved promotion with id: {Id}.", id);

        return result;
    }
}
