using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.RoleMenuItem;

namespace PharmacyManagementSystem.Server.RoleMenuItem;

public class RoleMenuItemRepository(ILogger<RoleMenuItemRepository> logger, IRoleMenuItemStorageClient storageClient) : IRoleMenuItemRepository
{
    private readonly ILogger<RoleMenuItemRepository> _logger = logger;
    private readonly IRoleMenuItemStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.RoleMenuItem.RoleMenuItem>?> GetByFilterCriteriaAsync(RoleMenuItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("Repository: Getting role menu items by filter.");
        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Retrieved {Count} role menu items.", result?.Count ?? 0);
        return result;
    }

    public async Task<Common.RoleMenuItem.RoleMenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("Repository: Getting role menu item by id: {Id}.", id);
        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Retrieved role menu item with id: {Id}.", id);
        return result;
    }

    public async Task<Common.RoleMenuItem.RoleMenuItem?> AddAsync(Common.RoleMenuItem.RoleMenuItem? roleMenuItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleMenuItem);
        _logger.LogDebug("Repository: Adding role menu item.");
        var result = await _storageClient.AddAsync(roleMenuItem, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Added role menu item.");
        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("Repository: Removing role menu item with id: {Id}.", id);
        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Repository: Removed role menu item with id: {Id}.", id);
    }
}
