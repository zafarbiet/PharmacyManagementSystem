namespace PharmacyManagementSystem.Server.UserRole;

public interface ISaveUserRoleAction
{
    Task<Common.UserRole.UserRole?> AddAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken);
    Task<Common.UserRole.UserRole?> UpdateAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
