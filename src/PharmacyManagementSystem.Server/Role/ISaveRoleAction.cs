namespace PharmacyManagementSystem.Server.Role;

public interface ISaveRoleAction
{
    Task<Common.Role.Role?> AddAsync(Common.Role.Role? role, CancellationToken cancellationToken);
    Task<Common.Role.Role?> UpdateAsync(Common.Role.Role? role, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
