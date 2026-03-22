using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerSubscription;

namespace PharmacyManagementSystem.Server.CustomerSubscription;

public class GetCustomerSubscriptionAction(ILogger<GetCustomerSubscriptionAction> logger, ICustomerSubscriptionRepository repository) : IGetCustomerSubscriptionAction
{
    private readonly ILogger<GetCustomerSubscriptionAction> _logger = logger;
    private readonly ICustomerSubscriptionRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.CustomerSubscription.CustomerSubscription>?> GetByFilterCriteriaAsync(CustomerSubscriptionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting customer subscriptions by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} customer subscriptions.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting customer subscription by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved customer subscription with id: {Id}.", id);

        return result;
    }
}
