using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.SubscriptionFulfillment;

public class SaveSubscriptionFulfillmentAction(ILogger<SaveSubscriptionFulfillmentAction> logger, ISubscriptionFulfillmentRepository repository) : ISaveSubscriptionFulfillmentAction
{
    private readonly ILogger<SaveSubscriptionFulfillmentAction> _logger = logger;
    private readonly ISubscriptionFulfillmentRepository _repository = repository;

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> AddAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);

        if (subscriptionFulfillment.SubscriptionId == Guid.Empty)
            throw new BadRequestException("SubscriptionFulfillment SubscriptionId is required.");

        if (string.IsNullOrWhiteSpace(subscriptionFulfillment.Status))
            throw new BadRequestException("SubscriptionFulfillment Status is required.");

        subscriptionFulfillment.UpdatedBy = "system";

        _logger.LogDebug("Adding new subscription fulfillment.");

        var result = await _repository.AddAsync(subscriptionFulfillment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added subscription fulfillment.");

        return result;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> UpdateAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);

        if (subscriptionFulfillment.SubscriptionId == Guid.Empty)
            throw new BadRequestException("SubscriptionFulfillment SubscriptionId is required.");

        if (string.IsNullOrWhiteSpace(subscriptionFulfillment.Status))
            throw new BadRequestException("SubscriptionFulfillment Status is required.");

        subscriptionFulfillment.UpdatedBy = "system";

        _logger.LogDebug("Updating subscription fulfillment with id: {Id}.", subscriptionFulfillment.Id);

        var result = await _repository.UpdateAsync(subscriptionFulfillment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated subscription fulfillment with id: {Id}.", subscriptionFulfillment.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing subscription fulfillment with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed subscription fulfillment with id: {Id}.", id);
    }
}
