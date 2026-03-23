namespace PharmacyManagementSystem.Common.Drug;

public class DrugFilter : FilterBase
{
    public string? Name { get; set; }
    public string? GenericName { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Composition { get; set; }
}
