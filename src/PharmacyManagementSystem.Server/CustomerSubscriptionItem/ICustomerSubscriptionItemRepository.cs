using PharmacyManagementSystem.Common.CustomerSubscriptionItem;
namespace PharmacyManagementSystem.Server.CustomerSubscriptionItem;
public interface ICustomerSubscriptionItemRepository
{
    Task<IReadOnlyCollection<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>?> GetByFilterCriteriaAsync(CustomerSubscriptionItemFilter filter, CancellationToken cancellationToken);
    Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> AddAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken);
    Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> UpdateAsync(Common.CustomerSubscriptionItem.CustomerSubscriptionItem? customerSubscriptionItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
