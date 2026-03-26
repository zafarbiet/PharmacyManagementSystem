using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.RoleMenuItem;

namespace PharmacyManagementSystem.Server.RoleMenuItem;

public class GetRoleMenuItemAction(ILogger<GetRoleMenuItemAction> logger, IRoleMenuItemRepository repository) : IGetRoleMenuItemAction
{
    private readonly ILogger<GetRoleMenuItemAction> _logger = logger;
    private readonly IRoleMenuItemRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.RoleMenuItem.RoleMenuItem>?> GetByFilterCriteriaAsync(RoleMenuItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("Getting role menu items by filter.");
        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Retrieved {Count} role menu items.", result?.Count ?? 0);
        return result;
    }

    public async Task<Common.RoleMenuItem.RoleMenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("Getting role menu item by id: {Id}.", id);
        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Retrieved role menu item with id: {Id}.", id);
        return result;
    }
}
