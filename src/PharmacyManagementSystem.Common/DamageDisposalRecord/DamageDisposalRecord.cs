namespace PharmacyManagementSystem.Common.DamageDisposalRecord;

public class DamageDisposalRecord : BaseObject
{
    public Guid Id { get; set; }
    public Guid DamageRecordId { get; set; }
    public DateTimeOffset DisposedAt { get; set; }
    public string? DisposalMethod { get; set; }
    public string? DisposedBy { get; set; }
    public string? Notes { get; set; }
}
