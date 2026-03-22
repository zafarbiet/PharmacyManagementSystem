using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.CustomerSubscriptionItem;

public class SaveCustomerSubscriptionItemAction(ILogger<SaveCustomerSubscriptionItemAction> logger, ICustomerSubscriptionItemRepository repository) : ISaveCustomerSubscriptionItemAction
{
    private readonly ILogger<SaveCustomerSubscriptionItemAction> _logger = logger;
    private readonly ICustomerSubscriptionItemRepository _repository = repository;

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> AddAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);

        if (customerSubscriptionItem.SubscriptionId == Guid.Empty)
            throw new BadRequestException("CustomerSubscriptionItem SubscriptionId is required.");

        if (customerSubscriptionItem.DrugId == Guid.Empty)
            throw new BadRequestException("CustomerSubscriptionItem DrugId is required.");

        if (customerSubscriptionItem.QuantityPerCycle <= 0)
            throw new BadRequestException("CustomerSubscriptionItem QuantityPerCycle must be greater than zero.");

        customerSubscriptionItem.UpdatedBy = "system";

        _logger.LogDebug("Adding new customer subscription item.");

        var result = await _repository.AddAsync(customerSubscriptionItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added customer subscription item.");

        return result;
    }

    public async Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> UpdateAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerSubscriptionItem);

        if (customerSubscriptionItem.SubscriptionId == Guid.Empty)
            throw new BadRequestException("CustomerSubscriptionItem SubscriptionId is required.");

        if (customerSubscriptionItem.DrugId == Guid.Empty)
            throw new BadRequestException("CustomerSubscriptionItem DrugId is required.");

        if (customerSubscriptionItem.QuantityPerCycle <= 0)
            throw new BadRequestException("CustomerSubscriptionItem QuantityPerCycle must be greater than zero.");

        customerSubscriptionItem.UpdatedBy = "system";

        _logger.LogDebug("Updating customer subscription item with id: {Id}.", customerSubscriptionItem.Id);

        var result = await _repository.UpdateAsync(customerSubscriptionItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated customer subscription item with id: {Id}.", customerSubscriptionItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing customer subscription item with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed customer subscription item with id: {Id}.", id);
    }
}
