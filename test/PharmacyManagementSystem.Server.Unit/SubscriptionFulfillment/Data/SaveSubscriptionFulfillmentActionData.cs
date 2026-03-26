namespace PharmacyManagementSystem.Server.Unit.SubscriptionFulfillment.Data;

public static class SaveSubscriptionFulfillmentActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var subscriptionId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.SubscriptionFulfillment.SubscriptionFulfillment { SubscriptionId = subscriptionId, Status = "Fulfilled" },
            new Common.SubscriptionFulfillment.SubscriptionFulfillment { Id = Guid.NewGuid(), SubscriptionId = subscriptionId, Status = "Fulfilled", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.SubscriptionFulfillment.SubscriptionFulfillment { SubscriptionId = Guid.Empty, Status = "Fulfilled" }
        };

        yield return new object[]
        {
            new Common.SubscriptionFulfillment.SubscriptionFulfillment { SubscriptionId = Guid.NewGuid(), Status = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var subscriptionId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.SubscriptionFulfillment.SubscriptionFulfillment { Id = id, SubscriptionId = subscriptionId, Status = "Failed" },
            new Common.SubscriptionFulfillment.SubscriptionFulfillment { Id = id, SubscriptionId = subscriptionId, Status = "Failed", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
