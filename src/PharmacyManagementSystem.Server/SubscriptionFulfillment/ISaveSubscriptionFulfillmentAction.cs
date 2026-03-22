namespace PharmacyManagementSystem.Server.SubscriptionFulfillment;
public interface ISaveSubscriptionFulfillmentAction
{
    Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> AddAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken);
    Task<Common.SubscriptionFulfillment.SubscriptionFulfillment?> UpdateAsync(Common.SubscriptionFulfillment.SubscriptionFulfillment? subscriptionFulfillment, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
