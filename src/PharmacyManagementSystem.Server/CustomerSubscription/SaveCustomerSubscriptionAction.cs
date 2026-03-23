using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerSubscription;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.CustomerSubscription;

public class SaveCustomerSubscriptionAction(ILogger<SaveCustomerSubscriptionAction> logger, ICustomerSubscriptionRepository repository) : ISaveCustomerSubscriptionAction
{
    private readonly ILogger<SaveCustomerSubscriptionAction> _logger = logger;
    private readonly ICustomerSubscriptionRepository _repository = repository;

    public async Task<Common.CustomerSubscription.CustomerSubscription?> AddAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);

        if (customerSubscription.PatientId == Guid.Empty)
            throw new BadRequestException("CustomerSubscription PatientId is required.");

        if (customerSubscription.CycleDayOfMonth < 1 || customerSubscription.CycleDayOfMonth > 28)
            throw new BadRequestException("CustomerSubscription CycleDayOfMonth must be between 1 and 28.");

        if (string.IsNullOrWhiteSpace(customerSubscription.Status))
            throw new BadRequestException("CustomerSubscription Status is required.");

        customerSubscription.UpdatedBy = "system";

        _logger.LogDebug("Adding new customer subscription for patient id: {PatientId}.", customerSubscription.PatientId);

        var result = await _repository.AddAsync(customerSubscription, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added customer subscription for patient id: {PatientId}.", customerSubscription.PatientId);

        return result;
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> UpdateAsync(Common.CustomerSubscription.CustomerSubscription? customerSubscription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscription);

        if (customerSubscription.PatientId == Guid.Empty)
            throw new BadRequestException("CustomerSubscription PatientId is required.");

        if (customerSubscription.CycleDayOfMonth < 1 || customerSubscription.CycleDayOfMonth > 28)
            throw new BadRequestException("CustomerSubscription CycleDayOfMonth must be between 1 and 28.");

        if (string.IsNullOrWhiteSpace(customerSubscription.Status))
            throw new BadRequestException("CustomerSubscription Status is required.");

        customerSubscription.UpdatedBy = "system";

        _logger.LogDebug("Updating customer subscription with id: {Id}.", customerSubscription.Id);

        var result = await _repository.UpdateAsync(customerSubscription, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated customer subscription with id: {Id}.", customerSubscription.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing customer subscription with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed customer subscription with id: {Id}.", id);
    }

    public async Task<Common.CustomerSubscription.CustomerSubscription?> ApproveAsync(Guid id, string approvedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(approvedBy);

        _logger.LogDebug("Approving customer subscription {Id} by {ApprovedBy}.", id, approvedBy);

        var subscription = await _repository.GetByIdAsync(id.ToString(), cancellationToken).ConfigureAwait(false);
        if (subscription is null)
            throw new BadRequestException($"Customer subscription {id} not found.");

        if (subscription.ApprovalStatus is "Approved")
            throw new ConflictException($"Customer subscription {id} is already approved.");

        subscription.ApprovalStatus = "Approved";
        subscription.ApprovedBy = approvedBy;
        subscription.ApprovedAt = DateTimeOffset.UtcNow;
        subscription.UpdatedBy = approvedBy;

        var result = await _repository.UpdateAsync(subscription, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Approved customer subscription {Id}.", id);

        return result;
    }

    public async Task<IReadOnlyCollection<Common.CustomerSubscription.CustomerSubscription>> ApproveBatchAsync(string approvedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(approvedBy);

        _logger.LogDebug("Batch approving pending customer subscriptions by {ApprovedBy}.", approvedBy);

        var pending = await _repository.GetByFilterCriteriaAsync(
            new CustomerSubscriptionFilter { ApprovalStatus = "Pending" },
            cancellationToken).ConfigureAwait(false);

        var approved = new List<Common.CustomerSubscription.CustomerSubscription>();

        foreach (var subscription in pending ?? [])
        {
            subscription.ApprovalStatus = "Approved";
            subscription.ApprovedBy = approvedBy;
            subscription.ApprovedAt = DateTimeOffset.UtcNow;
            subscription.UpdatedBy = approvedBy;

            var result = await _repository.UpdateAsync(subscription, cancellationToken).ConfigureAwait(false);
            if (result is not null)
                approved.Add(result);
        }

        _logger.LogDebug("Batch approved {Count} customer subscriptions.", approved.Count);

        return approved;
    }
}
