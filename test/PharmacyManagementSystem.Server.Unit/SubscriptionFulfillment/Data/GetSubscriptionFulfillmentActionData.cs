using PharmacyManagementSystem.Common.SubscriptionFulfillment;

namespace PharmacyManagementSystem.Server.Unit.SubscriptionFulfillment.Data;

public static class GetSubscriptionFulfillmentActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var subscriptionId = Guid.NewGuid();

        yield return new object[]
        {
            new SubscriptionFulfillmentFilter { SubscriptionId = subscriptionId },
            new List<Common.SubscriptionFulfillment.SubscriptionFulfillment>
            {
                new() { Id = Guid.NewGuid(), SubscriptionId = subscriptionId, Status = "Fulfilled", IsActive = true }
            }
        };

        yield return new object[]
        {
            new SubscriptionFulfillmentFilter(),
            new List<Common.SubscriptionFulfillment.SubscriptionFulfillment>
            {
                new() { Id = Guid.NewGuid(), SubscriptionId = subscriptionId, Status = "Fulfilled", IsActive = true },
                new() { Id = Guid.NewGuid(), SubscriptionId = Guid.NewGuid(), Status = "Pending", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.SubscriptionFulfillment.SubscriptionFulfillment { Id = id, SubscriptionId = Guid.NewGuid(), Status = "Fulfilled", IsActive = true }
        };
    }
}
