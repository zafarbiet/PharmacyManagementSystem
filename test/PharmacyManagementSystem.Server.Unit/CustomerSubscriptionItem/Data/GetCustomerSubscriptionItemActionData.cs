using PharmacyManagementSystem.Common.CustomerSubscriptionItem;

namespace PharmacyManagementSystem.Server.Unit.CustomerSubscriptionItem.Data;

public static class GetCustomerSubscriptionItemActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var subscriptionId = Guid.NewGuid();

        yield return new object[]
        {
            new CustomerSubscriptionItemFilter { SubscriptionId = subscriptionId },
            new List<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>
            {
                new() { Id = Guid.NewGuid(), SubscriptionId = subscriptionId, DrugId = Guid.NewGuid(), QuantityPerCycle = 30, IsActive = true }
            }
        };

        yield return new object[]
        {
            new CustomerSubscriptionItemFilter(),
            new List<Common.CustomerSubscriptionItem.CustomerSubscriptionItem>
            {
                new() { Id = Guid.NewGuid(), SubscriptionId = subscriptionId, DrugId = Guid.NewGuid(), QuantityPerCycle = 30, IsActive = true },
                new() { Id = Guid.NewGuid(), SubscriptionId = Guid.NewGuid(), DrugId = Guid.NewGuid(), QuantityPerCycle = 60, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.CustomerSubscriptionItem.CustomerSubscriptionItem { Id = id, SubscriptionId = Guid.NewGuid(), DrugId = Guid.NewGuid(), QuantityPerCycle = 30, IsActive = true }
        };
    }
}
