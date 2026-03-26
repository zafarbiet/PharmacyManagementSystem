using PharmacyManagementSystem.Common.RoleMenuItem;

namespace PharmacyManagementSystem.Server.RoleMenuItem;

public interface IGetRoleMenuItemAction
{
    Task<IReadOnlyCollection<Common.RoleMenuItem.RoleMenuItem>?> GetByFilterCriteriaAsync(RoleMenuItemFilter filter, CancellationToken cancellationToken);
    Task<Common.RoleMenuItem.RoleMenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
