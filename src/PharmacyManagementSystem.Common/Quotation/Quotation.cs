namespace PharmacyManagementSystem.Common.Quotation;

public class Quotation : BaseObject
{
    public Guid Id { get; set; }
    public Guid QuotationRequestId { get; set; }
    public Guid VendorId { get; set; }
    public DateTimeOffset QuotationDate { get; set; }
    public DateTimeOffset? ValidUntil { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
}
