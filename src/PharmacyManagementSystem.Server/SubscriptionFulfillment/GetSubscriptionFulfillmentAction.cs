using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.SubscriptionFulfillment;

namespace PharmacyManagementSystem.Server.SubscriptionFulfillment;

public class GetSubscriptionFulfillmentAction(ILogger<GetSubscriptionFulfillmentAction> logger, ISubscriptionFulfillmentRepository repository) : IGetSubscriptionFulfillmentAction
{
    private readonly ILogger<GetSubscriptionFulfillmentAction> _logger = logger;
    private readonly ISubscriptionFulfillmentRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.SubscriptionFulfillment.SubscriptionFulfillment>?> GetByFilterCriteriaAsync(SubscriptionFulfillmentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting subscription fulfillments by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} subscription fulfillments.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting subscription fulfillment by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved subscription fulfillment with id: {Id}.", id);

        return result;
    }
}
