using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerSubscriptionItem;

namespace PharmacyManagementSystem.Server.CustomerSubscriptionItem;

public class CustomerSubscriptionItemRepository(ILogger<CustomerSubscriptionItemRepository> logger, ICustomerSubscriptionItemStorageClient storageClient) : ICustomerSubscriptionItemRepository
{
    private readonly ILogger<CustomerSubscriptionItemRepository> _logger = logger;
    private readonly ICustomerSubscriptionItemStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>?> GetByFilterCriteriaAsync(CustomerSubscriptionItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting customer subscription items by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} customer subscription items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting customer subscription item by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved customer subscription item with id: {Id}.", id);

        return result;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> AddAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);

        _logger.LogDebug("Repository: Adding customer subscription item.");

        var result = await _storageClient.AddAsync(customerSubscriptionItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added customer subscription item.");

        return result;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> UpdateAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);

        _logger.LogDebug("Repository: Updating customer subscription item with id: {Id}.", customerSubscriptionItem.Id);

        var result = await _storageClient.UpdateAsync(customerSubscriptionItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated customer subscription item with id: {Id}.", customerSubscriptionItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing customer subscription item with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed customer subscription item with id: {Id}.", id);
    }
}
