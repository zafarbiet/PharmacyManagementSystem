using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.UserRole;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.UserRole;

namespace PharmacyManagementSystem.Server.Data.SqlServer.UserRole;

public class SqlServerUserRoleStorageClient(ILogger<SqlServerUserRoleStorageClient> logger, ISqlServerDbClient dbClient) : IUserRoleStorageClient
{
    private readonly ILogger<SqlServerUserRoleStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.UserRole.UserRole>?> GetByFilterCriteriaAsync(UserRoleFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting user roles by filter criteria.");

        var sql = await UserRoleDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.UserRole.UserRole>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} user roles.", list.Count);

        return list;
    }

    public async Task<Common.UserRole.UserRole?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting user role by id: {Id}.", id);

        var sql = await UserRoleDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.UserRole.UserRole>(sql, cancellationToken).ConfigureAwait(false);

        var userRole = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved user role with id: {Id}.", id);

        return userRole;
    }

    public async Task<Common.UserRole.UserRole?> AddAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userRole);

        _logger.LogDebug("StorageClient: Adding user role for user: {UserId}.", userRole.UserId);

        var sql = await UserRoleDatabaseCommandText.GetInsertSql(userRole).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.UserRole.UserRole>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added user role for user: {UserId}.", userRole.UserId);

        return inserted;
    }

    public async Task<Common.UserRole.UserRole?> UpdateAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userRole);

        _logger.LogDebug("StorageClient: Updating user role with id: {Id}.", userRole.Id);

        var sql = await UserRoleDatabaseCommandText.GetUpdateSql(userRole).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.UserRole.UserRole>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated user role with id: {Id}.", userRole.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing user role with id: {Id}.", id);

        var sql = await UserRoleDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed user role with id: {Id}.", id);
    }
}
