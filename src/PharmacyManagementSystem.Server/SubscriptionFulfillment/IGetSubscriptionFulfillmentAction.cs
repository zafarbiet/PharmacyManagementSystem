using PharmacyManagementSystem.Common.SubscriptionFulfillment;
namespace PharmacyManagementSystem.Server.SubscriptionFulfillment;
public interface IGetSubscriptionFulfillmentAction
{
    Task<IReadOnlyCollection<Common.SubscriptionFulfillment.SubscriptionFulfillment>?> GetByFilterCriteriaAsync(SubscriptionFulfillmentFilter filter, CancellationToken cancellationToken);
    Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
