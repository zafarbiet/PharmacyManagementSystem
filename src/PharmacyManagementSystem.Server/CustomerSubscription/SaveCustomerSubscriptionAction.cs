using Microsoft.Extensions.Logging;
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
}
