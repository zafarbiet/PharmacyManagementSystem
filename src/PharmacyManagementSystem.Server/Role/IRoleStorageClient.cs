using PharmacyManagementSystem.Common.Role;

namespace PharmacyManagementSystem.Server.Role;

public interface IRoleStorageClient
{
    Task<IReadOnlyCollection<Common.Role.Role>?> GetByFilterCriteriaAsync(RoleFilter filter, CancellationToken cancellationToken);
    Task<Common.Role.Role?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Role.Role?> AddAsync(Common.Role.Role? role, CancellationToken cancellationToken);
    Task<Common.Role.Role?> UpdateAsync(Common.Role.Role? role, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
