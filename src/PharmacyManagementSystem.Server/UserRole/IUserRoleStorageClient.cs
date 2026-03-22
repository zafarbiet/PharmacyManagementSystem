using PharmacyManagementSystem.Common.UserRole;

namespace PharmacyManagementSystem.Server.UserRole;

public interface IUserRoleStorageClient
{
    Task<IReadOnlyCollection<Common.UserRole.UserRole>?> GetByFilterCriteriaAsync(UserRoleFilter filter, CancellationToken cancellationToken);
    Task<Common.UserRole.UserRole?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.UserRole.UserRole?> AddAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken);
    Task<Common.UserRole.UserRole?> UpdateAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
