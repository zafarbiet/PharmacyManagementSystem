namespace PharmacyManagementSystem.Common.PaymentLedger;

public class PaymentLedgerFilter : FilterBase
{
    public Guid? VendorId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public string? Status { get; set; }
}
