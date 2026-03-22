using PharmacyManagementSystem.Common.CustomerSubscriptionItem;
namespace PharmacyManagementSystem.Server.CustomerSubscriptionItem;
public interface IGetCustomerSubscriptionItemAction
{
    Task<IReadOnlyCollection<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>?> GetByFilterCriteriaAsync(CustomerSubscriptionItemFilter filter, CancellationToken cancellationToken);
    Task<Common.CustomerSubscriptionItem.CustomerSubscriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
