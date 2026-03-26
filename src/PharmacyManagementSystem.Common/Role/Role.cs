namespace PharmacyManagementSystem.Common.Role;

public class Role : BaseObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public long Permissions { get; set; }
}
