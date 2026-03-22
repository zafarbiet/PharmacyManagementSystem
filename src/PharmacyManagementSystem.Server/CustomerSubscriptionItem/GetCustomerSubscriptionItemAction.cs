using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerSubscriptionItem;

namespace PharmacyManagementSystem.Server.CustomerSubscriptionItem;

public class GetCustomerSubscriptionItemAction(ILogger<GetCustomerSubscriptionItemAction> logger, ICustomerSubscriptionItemRepository repository) : IGetCustomerSubscriptionItemAction
{
    private readonly ILogger<GetCustomerSubscriptionItemAction> _logger = logger;
    private readonly ICustomerSubscriptionItemRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>?> GetByFilterCriteriaAsync(CustomerSubscriptionItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting customer subscription items by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} customer subscription items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting customer subscription item by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved customer subscription item with id: {Id}.", id);

        return result;
    }
}
