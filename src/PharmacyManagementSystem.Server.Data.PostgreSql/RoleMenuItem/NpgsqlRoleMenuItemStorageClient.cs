using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.RoleMenuItem;
using PharmacyManagementSystem.Server.RoleMenuItem;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.RoleMenuItem;

public class NpgsqlRoleMenuItemStorageClient(ILogger<NpgsqlRoleMenuItemStorageClient> logger, INpgsqlDbClient dbClient) : IRoleMenuItemStorageClient
{
    private readonly ILogger<NpgsqlRoleMenuItemStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.RoleMenuItem.RoleMenuItem>?> GetByFilterCriteriaAsync(RoleMenuItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting role menu items by filter.");
        var sql = await RoleMenuItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.RoleMenuItem.RoleMenuItem>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} role menu items.", list.Count);
        return list;
    }

    public async Task<Common.RoleMenuItem.RoleMenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting role menu item by id: {Id}.", id);
        var sql = await RoleMenuItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.RoleMenuItem.RoleMenuItem>(sql, cancellationToken).ConfigureAwait(false);
        return result.FirstOrDefault();
    }

    public async Task<Common.RoleMenuItem.RoleMenuItem?> AddAsync(Common.RoleMenuItem.RoleMenuItem? roleMenuItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleMenuItem);
        _logger.LogDebug("StorageClient: Adding role menu item.");
        var sql = await RoleMenuItemDatabaseCommandText.GetInsertSql(roleMenuItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.RoleMenuItem.RoleMenuItem>(sql, cancellationToken).ConfigureAwait(false);
        return result.FirstOrDefault();
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing role menu item with id: {Id}.", id);
        var sql = await RoleMenuItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
    }
}
