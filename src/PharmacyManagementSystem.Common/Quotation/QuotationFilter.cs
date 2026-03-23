namespace PharmacyManagementSystem.Common.Quotation;

public class QuotationFilter : FilterBase
{
    public Guid? QuotationRequestId { get; set; }
    public Guid? VendorId { get; set; }
    public string? Status { get; set; }
}
