namespace PharmacyManagementSystem.Common.PurchaseOrder;

public class PurchaseOrderFilter : FilterBase
{
    public Guid VendorId { get; set; }
    public string? Status { get; set; }
    public Guid? ParentPurchaseOrderId { get; set; }
}
