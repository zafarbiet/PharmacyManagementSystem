using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Role;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Role;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Role;

public class SqlServerRoleStorageClient(ILogger<SqlServerRoleStorageClient> logger, ISqlServerDbClient dbClient) : IRoleStorageClient
{
    private readonly ILogger<SqlServerRoleStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Role.Role>?> GetByFilterCriteriaAsync(RoleFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting roles by filter criteria.");

        var sql = await RoleDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Role.Role>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} roles.", list.Count);

        return list;
    }

    public async Task<Common.Role.Role?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting role by id: {Id}.", id);

        var sql = await RoleDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Role.Role>(sql, cancellationToken).ConfigureAwait(false);

        var role = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved role with id: {Id}.", id);

        return role;
    }

    public async Task<Common.Role.Role?> AddAsync(Common.Role.Role? role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role);

        _logger.LogDebug("StorageClient: Adding role with name: {Name}.", role.Name);

        var sql = await RoleDatabaseCommandText.GetInsertSql(role).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Role.Role>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added role with name: {Name}.", role.Name);

        return inserted;
    }

    public async Task<Common.Role.Role?> UpdateAsync(Common.Role.Role? role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role);

        _logger.LogDebug("StorageClient: Updating role with id: {Id}.", role.Id);

        var sql = await RoleDatabaseCommandText.GetUpdateSql(role).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Role.Role>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated role with id: {Id}.", role.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing role with id: {Id}.", id);

        var sql = await RoleDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed role with id: {Id}.", id);
    }
}
