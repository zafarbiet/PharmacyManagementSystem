using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.SubscriptionFulfillment;

namespace PharmacyManagementSystem.Server.SubscriptionFulfillment;

public class SubscriptionFulfillmentRepository(ILogger<SubscriptionFulfillmentRepository> logger, ISubscriptionFulfillmentStorageClient storageClient) : ISubscriptionFulfillmentRepository
{
    private readonly ILogger<SubscriptionFulfillmentRepository> _logger = logger;
    private readonly ISubscriptionFulfillmentStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.SubscriptionFulfillment.SubscriptionFulfillment>?> GetByFilterCriteriaAsync(SubscriptionFulfillmentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting subscription fulfillments by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} subscription fulfillments.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting subscription fulfillment by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved subscription fulfillment with id: {Id}.", id);

        return result;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> AddAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);

        _logger.LogDebug("Repository: Adding subscription fulfillment.");

        var result = await _storageClient.AddAsync(subscriptionFulfillment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added subscription fulfillment.");

        return result;
    }

    public async Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> UpdateAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFulfillment);

        _logger.LogDebug("Repository: Updating subscription fulfillment with id: {Id}.", subscriptionFulfillment.Id);

        var result = await _storageClient.UpdateAsync(subscriptionFulfillment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated subscription fulfillment with id: {Id}.", subscriptionFulfillment.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing subscription fulfillment with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed subscription fulfillment with id: {Id}.", id);
    }
}
