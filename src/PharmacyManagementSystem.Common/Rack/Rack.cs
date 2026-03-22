namespace PharmacyManagementSystem.Common.Rack;

public class Rack : BaseObject
{
    public Guid Id { get; set; }
    public Guid StorageZoneId { get; set; }
    public string? Label { get; set; }
    public string? Description { get; set; }
    public int? Capacity { get; set; }
}
