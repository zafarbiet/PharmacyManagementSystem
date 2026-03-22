using PharmacyManagementSystem.Common.SubscriptionFulfillment;
namespace PharmacyManagementSystem.Server.SubscriptionFulfillment;
public interface ISubscriptionFulfillmentRepository
{
    Task<IReadOnlyCollection<Common.SubscriptionFulfillment.SubscriptionFulfillment>?> GetByFilterCriteriaAsync(SubscriptionFulfillmentFilter filter, CancellationToken cancellationToken);
    Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> AddAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken);
    Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> UpdateAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
