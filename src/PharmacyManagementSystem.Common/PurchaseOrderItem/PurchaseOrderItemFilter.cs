namespace PharmacyManagementSystem.Common.PurchaseOrderItem;

public class PurchaseOrderItemFilter : FilterBase
{
    public Guid? PurchaseOrderId { get; set; }
    public Guid? DrugId { get; set; }
}
