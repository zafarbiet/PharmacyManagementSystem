namespace PharmacyManagementSystem.Common.PaymentLedger;

public class PaymentLedger : BaseObject
{
    public Guid Id { get; set; }
    public Guid VendorId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public decimal InvoicedAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}
