namespace PharmacyManagementSystem.Common.DisposalRecord;

public class DisposalRecord : BaseObject
{
    public Guid Id { get; set; }
    public Guid ExpiryRecordId { get; set; }
    public DateTimeOffset DisposedAt { get; set; }
    public int QuantityDisposed { get; set; }
    public string? DisposalMethod { get; set; }
    public string? DisposedBy { get; set; }
    public string? WitnessedBy { get; set; }
    public string? RegulatoryReferenceNumber { get; set; }
    public string? Notes { get; set; }
}
