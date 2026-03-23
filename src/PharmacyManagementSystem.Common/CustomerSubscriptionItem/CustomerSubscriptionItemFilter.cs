namespace PharmacyManagementSystem.Common.CustomerSubscriptionItem;

public class CustomerSubscriptionItemFilter : FilterBase
{
    public Guid? SubscriptionId { get; set; }
    public Guid? DrugId { get; set; }
}
