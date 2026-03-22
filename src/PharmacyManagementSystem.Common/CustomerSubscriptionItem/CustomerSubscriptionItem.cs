namespace PharmacyManagementSystem.Common.CustomerSubscriptionItem;

public class CustomerSubscriptionItem : BaseObject
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public Guid DrugId { get; set; }
    public int QuantityPerCycle { get; set; }
    public Guid? PrescriptionId { get; set; }
}
