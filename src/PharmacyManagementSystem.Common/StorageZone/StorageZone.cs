namespace PharmacyManagementSystem.Common.StorageZone;

public class StorageZone : BaseObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ZoneType { get; set; }
    public string? Description { get; set; }
    public decimal? TemperatureRangeMin { get; set; }
    public decimal? TemperatureRangeMax { get; set; }
}
