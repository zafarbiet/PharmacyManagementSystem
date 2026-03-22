namespace PharmacyManagementSystem.Common.ExpiryRecord;

public class ExpiryRecord : BaseObject
{
    public Guid Id { get; set; }
    public Guid DrugInventoryId { get; set; }
    public DateTimeOffset DetectedAt { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public int QuantityAffected { get; set; }
    public string? Status { get; set; }
    public string? InitiatedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public Guid? QuarantineRackId { get; set; }
    public string? Notes { get; set; }
}
