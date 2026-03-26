namespace PharmacyManagementSystem.Server.RoleMenuItem;

public interface ISaveRoleMenuItemAction
{
    Task<Common.RoleMenuItem.RoleMenuItem?> AddAsync(Common.RoleMenuItem.RoleMenuItem? roleMenuItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
