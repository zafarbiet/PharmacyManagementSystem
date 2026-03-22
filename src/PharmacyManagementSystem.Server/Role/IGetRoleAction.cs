using PharmacyManagementSystem.Common.Role;

namespace PharmacyManagementSystem.Server.Role;

public interface IGetRoleAction
{
    Task<IReadOnlyCollection<Common.Role.Role>?> GetByFilterCriteriaAsync(RoleFilter filter, CancellationToken cancellationToken);
    Task<Common.Role.Role?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
