using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.MenuItem;

namespace PharmacyManagementSystem.Server.MenuItem;

public class MenuItemRepository(ILogger<MenuItemRepository> logger, IMenuItemStorageClient storageClient) : IMenuItemRepository
{
    private readonly ILogger<MenuItemRepository> _logger = logger;
    private readonly IMenuItemStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetByFilterCriteriaAsync(MenuItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("Repository: Getting menu items by filter criteria.");
        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Retrieved {Count} menu items.", result?.Count ?? 0);
        return result;
    }

    public async Task<Common.MenuItem.MenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("Repository: Getting menu item by id: {Id}.", id);
        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Retrieved menu item with id: {Id}.", id);
        return result;
    }

    public async Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetForUserAsync(string username, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(username);
        _logger.LogDebug("Repository: Getting menu items for user: {Username}.", username);
        var result = await _storageClient.GetForUserAsync(username, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Retrieved {Count} menu items for user: {Username}.", result?.Count ?? 0, username);
        return result;
    }

    public async Task<Common.MenuItem.MenuItem?> AddAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(menuItem);
        _logger.LogDebug("Repository: Adding menu item with key: {Key}.", menuItem.Key);
        var result = await _storageClient.AddAsync(menuItem, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Added menu item with key: {Key}.", menuItem.Key);
        return result;
    }

    public async Task<Common.MenuItem.MenuItem?> UpdateAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(menuItem);
        _logger.LogDebug("Repository: Updating menu item with id: {Id}.", menuItem.Id);
        var result = await _storageClient.UpdateAsync(menuItem, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Updated menu item with id: {Id}.", menuItem.Id);
        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("Repository: Removing menu item with id: {Id}.", id);
        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Removed menu item with id: {Id}.", id);
    }
}
