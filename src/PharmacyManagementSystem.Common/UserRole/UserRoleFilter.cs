namespace PharmacyManagementSystem.Common.UserRole;

public class UserRoleFilter : FilterBase
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
