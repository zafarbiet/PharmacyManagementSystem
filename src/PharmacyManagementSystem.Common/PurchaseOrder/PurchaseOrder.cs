namespace PharmacyManagementSystem.Common.PurchaseOrder;

public class PurchaseOrder : BaseObject
{
    public Guid Id { get; set; }
    public Guid VendorId { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public decimal TotalAmount { get; set; }
    public Guid? QuotationId { get; set; }
    public string? PoNumber { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public Guid? ParentPurchaseOrderId { get; set; }
    public Guid? BranchId { get; set; }
}
