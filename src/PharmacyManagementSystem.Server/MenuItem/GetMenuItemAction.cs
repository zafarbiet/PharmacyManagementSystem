using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.MenuItem;

namespace PharmacyManagementSystem.Server.MenuItem;

public class GetMenuItemAction(ILogger<GetMenuItemAction> logger, IMenuItemRepository repository) : IGetMenuItemAction
{
    private readonly ILogger<GetMenuItemAction> _logger = logger;
    private readonly IMenuItemRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetByFilterCriteriaAsync(MenuItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("Getting menu items by filter criteria.");
        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Retrieved {Count} menu items.", result?.Count ?? 0);
        return result;
    }

    public async Task<Common.MenuItem.MenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("Getting menu item by id: {Id}.", id);
        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Retrieved menu item with id: {Id}.", id);
        return result;
    }

    public async Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetForUserAsync(string username, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(username);
        _logger.LogDebug("Getting menu items for user: {Username}.", username);
        var result = await _repository.GetForUserAsync(username, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Retrieved {Count} menu items for user: {Username}.", result?.Count ?? 0, username);
        return result;
    }
}
