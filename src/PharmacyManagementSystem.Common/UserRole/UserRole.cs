namespace PharmacyManagementSystem.Common.UserRole;

public class UserRole : BaseObject
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTimeOffset AssignedAt { get; set; }
}
