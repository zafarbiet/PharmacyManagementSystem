using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.MenuItem;
using PharmacyManagementSystem.Server.MenuItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.MenuItem;

public class NpgsqlMenuItemStorageClient(ILogger<NpgsqlMenuItemStorageClient> logger, INpgsqlDbClient dbClient) : IMenuItemStorageClient
{
    private readonly ILogger<NpgsqlMenuItemStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetByFilterCriteriaAsync(MenuItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting menu items by filter criteria.");
        var sql = await MenuItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.MenuItem.MenuItem>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} menu items.", list.Count);
        return list;
    }

    public async Task<Common.MenuItem.MenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting menu item by id: {Id}.", id);
        var sql = await MenuItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.MenuItem.MenuItem>(sql, cancellationToken).ConfigureAwait(false);
        return result.FirstOrDefault();
    }

    public async Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetForUserAsync(string username, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(username);
        _logger.LogDebug("StorageClient: Getting menu items for user: {Username}.", username);
        var sql = await MenuItemDatabaseCommandText.GetForUserSql(username).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.MenuItem.MenuItem>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} menu items for user: {Username}.", list.Count, username);
        return list;
    }

    public async Task<Common.MenuItem.MenuItem?> AddAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(menuItem);
        _logger.LogDebug("StorageClient: Adding menu item with key: {Key}.", menuItem.Key);
        var sql = await MenuItemDatabaseCommandText.GetInsertSql(menuItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.MenuItem.MenuItem>(sql, cancellationToken).ConfigureAwait(false);
        return result.FirstOrDefault();
    }

    public async Task<Common.MenuItem.MenuItem?> UpdateAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(menuItem);
        _logger.LogDebug("StorageClient: Updating menu item with id: {Id}.", menuItem.Id);
        var sql = await MenuItemDatabaseCommandText.GetUpdateSql(menuItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.MenuItem.MenuItem>(sql, cancellationToken).ConfigureAwait(false);
        return result.FirstOrDefault();
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing menu item with id: {Id}.", id);
        var sql = await MenuItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
    }
}
