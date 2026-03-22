namespace PharmacyManagementSystem.Common.SubscriptionFulfillment;

public class SubscriptionFulfillmentFilter : FilterBase
{
    public Guid SubscriptionId { get; set; }
    public string? Status { get; set; }
}
