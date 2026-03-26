namespace PharmacyManagementSystem.Common.RoleMenuItem;

public class RoleMenuItem : BaseObject
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Guid MenuItemId { get; set; }
}
