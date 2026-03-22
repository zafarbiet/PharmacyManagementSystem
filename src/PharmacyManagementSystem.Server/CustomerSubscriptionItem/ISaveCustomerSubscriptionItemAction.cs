namespace PharmacyManagementSystem.Server.CustomerSubscriptionItem;
public interface ISaveCustomerSubscriptionItemAction
{
    Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> AddAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken);
    Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> UpdateAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
