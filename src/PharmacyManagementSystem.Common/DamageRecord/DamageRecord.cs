namespace PharmacyManagementSystem.Common.DamageRecord;

public class DamageRecord : BaseObject
{
    public Guid Id { get; set; }
    public Guid DrugInventoryId { get; set; }
    public int QuantityDamaged { get; set; }
    public string? DamageType { get; set; }
    public DateTimeOffset DamagedAt { get; set; }
    public string? DiscoveredBy { get; set; }
    public string? Status { get; set; }
    public Guid? QuarantineRackId { get; set; }
    public Guid? StockTransactionId { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public string? Notes { get; set; }
}
