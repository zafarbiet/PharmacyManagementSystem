using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.RoleMenuItem;

public class SaveRoleMenuItemAction(ILogger<SaveRoleMenuItemAction> logger, IRoleMenuItemRepository repository) : ISaveRoleMenuItemAction
{
    private readonly ILogger<SaveRoleMenuItemAction> _logger = logger;
    private readonly IRoleMenuItemRepository _repository = repository;

    public async Task<Common.RoleMenuItem.RoleMenuItem?> AddAsync(Common.RoleMenuItem.RoleMenuItem? roleMenuItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleMenuItem);

        if (roleMenuItem.RoleId == Guid.Empty)
            throw new BadRequestException("RoleMenuItem RoleId is required.");

        if (roleMenuItem.MenuItemId == Guid.Empty)
            throw new BadRequestException("RoleMenuItem MenuItemId is required.");

        roleMenuItem.UpdatedBy = "system";

        _logger.LogDebug("Adding role menu item for role {RoleId} and menu item {MenuItemId}.", roleMenuItem.RoleId, roleMenuItem.MenuItemId);
        var result = await _repository.AddAsync(roleMenuItem, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Added role menu item for role {RoleId}.", roleMenuItem.RoleId);
        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("Removing role menu item with id: {Id}.", id);
        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Removed role menu item with id: {Id}.", id);
    }
}
