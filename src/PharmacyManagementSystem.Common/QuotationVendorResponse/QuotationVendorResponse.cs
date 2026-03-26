namespace PharmacyManagementSystem.Common.QuotationVendorResponse;

public class QuotationVendorResponse : BaseObject
{
    public Guid Id { get; set; }
    public Guid QuotationRequestId { get; set; }
    public Guid VendorId { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? RespondedAt { get; set; }
    public string? Notes { get; set; }
}
