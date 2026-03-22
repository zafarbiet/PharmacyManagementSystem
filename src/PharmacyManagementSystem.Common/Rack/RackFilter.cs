namespace PharmacyManagementSystem.Common.Rack;

public class RackFilter : FilterBase
{
    public Guid StorageZoneId { get; set; }
    public string? Label { get; set; }
}
