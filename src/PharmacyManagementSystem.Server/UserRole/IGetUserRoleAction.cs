using PharmacyManagementSystem.Common.UserRole;

namespace PharmacyManagementSystem.Server.UserRole;

public interface IGetUserRoleAction
{
    Task<IReadOnlyCollection<Common.UserRole.UserRole>?> GetByFilterCriteriaAsync(UserRoleFilter filter, CancellationToken cancellationToken);
    Task<Common.UserRole.UserRole?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
