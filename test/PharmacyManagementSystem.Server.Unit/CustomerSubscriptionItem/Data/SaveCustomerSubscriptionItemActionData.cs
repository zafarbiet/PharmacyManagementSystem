namespace PharmacyManagementSystem.Server.Unit.CustomerSubscriptionItem.Data;

public static class SaveCustomerSubscriptionItemActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var subscriptionId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.CustomerSubscriptionItem.CustomerSubscriptionItem { SubscriptionId = subscriptionId, DrugId = drugId, QuantityPerCycle = 30 },
            new Common.CustomerSubscriptionItem.CustomerSubscriptionItem { Id = Guid.NewGuid(), SubscriptionId = subscriptionId, DrugId = drugId, QuantityPerCycle = 30, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.CustomerSubscriptionItem.CustomerSubscriptionItem { SubscriptionId = Guid.Empty, DrugId = Guid.NewGuid(), QuantityPerCycle = 30 }
        };

        yield return new object[]
        {
            new Common.CustomerSubscriptionItem.CustomerSubscriptionItem { SubscriptionId = Guid.NewGuid(), DrugId = Guid.Empty, QuantityPerCycle = 30 }
        };

        yield return new object[]
        {
            new Common.CustomerSubscriptionItem.CustomerSubscriptionItem { SubscriptionId = Guid.NewGuid(), DrugId = Guid.NewGuid(), QuantityPerCycle = 0 }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var subscriptionId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.CustomerSubscriptionItem.CustomerSubscriptionItem { Id = id, SubscriptionId = subscriptionId, DrugId = drugId, QuantityPerCycle = 60 },
            new Common.CustomerSubscriptionItem.CustomerSubscriptionItem { Id = id, SubscriptionId = subscriptionId, DrugId = drugId, QuantityPerCycle = 60, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
