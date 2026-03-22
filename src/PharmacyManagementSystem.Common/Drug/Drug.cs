namespace PharmacyManagementSystem.Common.Drug;

public class Drug : BaseObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? GenericName { get; set; }
    public string? ManufacturerName { get; set; }
    public Guid CategoryId { get; set; }
    public string? UnitOfMeasure { get; set; }
    public int ReorderLevel { get; set; }
}
