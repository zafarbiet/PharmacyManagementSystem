namespace PharmacyManagementSystem.Common.QuotationVendorResponse;

public class QuotationVendorResponseFilter : FilterBase
{
    public Guid? QuotationRequestId { get; set; }
    public Guid? VendorId { get; set; }
    public string? Status { get; set; }
}
