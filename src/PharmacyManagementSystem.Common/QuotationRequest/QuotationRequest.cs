namespace PharmacyManagementSystem.Common.QuotationRequest;

public class QuotationRequest : BaseObject
{
    public Guid Id { get; set; }
    public DateTimeOffset RequestDate { get; set; }
    public DateTimeOffset? RequiredByDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public string? RequestedBy { get; set; }
}
