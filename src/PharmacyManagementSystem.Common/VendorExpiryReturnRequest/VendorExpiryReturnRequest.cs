namespace PharmacyManagementSystem.Common.VendorExpiryReturnRequest;

public class VendorExpiryReturnRequest : BaseObject
{
    public Guid Id { get; set; }
    public Guid ExpiryRecordId { get; set; }
    public Guid VendorId { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public int QuantityToReturn { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? VendorResponseAt { get; set; }
    public string? VendorNotes { get; set; }
}
