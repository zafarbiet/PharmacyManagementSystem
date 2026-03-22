namespace PharmacyManagementSystem.Common.SubscriptionFulfillment;

public class SubscriptionFulfillment : BaseObject
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public DateTimeOffset FulfillmentDate { get; set; }
    public string? Status { get; set; }
    public Guid? InvoiceId { get; set; }
    public string? Notes { get; set; }
}
