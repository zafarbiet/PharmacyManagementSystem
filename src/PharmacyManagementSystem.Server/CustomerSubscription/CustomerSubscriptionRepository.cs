using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerSubscription;

namespace PharmacyManagementSystem.Server.CustomerSubscription;

public class CustomerSubscriptionRepository(ILogger<CustomerSubscriptionRepository> logger, ICustomerSubscriptionStorageClient storageClient) : ICustomerSubscriptionRepository
{
    private readonly ILogger<CustomerSubscriptionRepository> _logger = logger;
    private readonly ICustomerSubscriptionStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.CustomerSubscription.CustomerSubscription>?> GetByFilterCriteriaAsync(CustomerSubscriptionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting customer subscriptions by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} customer subscriptions.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting customer subscription by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved customer subscription with id: {Id}.", id);

        return result;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> AddAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);

        _logger.LogDebug("Repository: Adding customer subscription for patient id: {PatientId}.", customerSubscription.PatientId);

        var result = await _storageClient.AddAsync(customerSubscription, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added customer subscription for patient id: {PatientId}.", customerSubscription.PatientId);

        return result;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> UpdateAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);

        _logger.LogDebug("Repository: Updating customer subscription with id: {Id}.", customerSubscription.Id);

        var result = await _storageClient.UpdateAsync(customerSubscription, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated customer subscription with id: {Id}.", customerSubscription.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing customer subscription with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed customer subscription with id: {Id}.", id);
    }
}
