using PharmacyManagementSystem.Common.RoleMenuItem;

namespace PharmacyManagementSystem.Server.RoleMenuItem;

public interface IRoleMenuItemRepository
{
    Task<IReadOnlyCollection<Common.RoleMenuItem.RoleMenuItem>?> GetByFilterCriteriaAsync(RoleMenuItemFilter filter, CancellationToken cancellationToken);
    Task<Common.RoleMenuItem.RoleMenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.RoleMenuItem.RoleMenuItem?> AddAsync(Common.RoleMenuItem.RoleMenuItem? roleMenuItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
