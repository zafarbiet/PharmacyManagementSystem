namespace PharmacyManagementSystem.Common.MenuItem;

public class MenuItem : BaseObject
{
    public Guid Id { get; set; }
    public string? Key { get; set; }
    public string? Label { get; set; }
    public string? Icon { get; set; }
    public string? ParentKey { get; set; }
    public int OrderIndex { get; set; }
}
