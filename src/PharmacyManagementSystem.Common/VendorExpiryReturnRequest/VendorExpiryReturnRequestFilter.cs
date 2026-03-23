namespace PharmacyManagementSystem.Common.VendorExpiryReturnRequest;

public class VendorExpiryReturnRequestFilter : FilterBase
{
    public Guid? ExpiryRecordId { get; set; }
    public Guid? VendorId { get; set; }
    public string? Status { get; set; }
}
