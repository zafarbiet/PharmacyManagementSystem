using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.UserRole;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.UserRole;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.UserRole;

public class NpgsqlUserRoleStorageClient(ILogger<NpgsqlUserRoleStorageClient> logger, INpgsqlDbClient dbClient) : IUserRoleStorageClient
{
    private readonly ILogger<NpgsqlUserRoleStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.UserRole.UserRole?>?> GetByFilterCriteriaAsync(UserRoleFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting userRoles by filter criteria.");
        var sql = await UserRoleDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.UserRole.UserRole>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} userRoles.", list.Count);
        return list;
    }

    public async Task<Common.UserRole.UserRole?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting userRole by id: {Id}.", id);
        var sql = await UserRoleDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.UserRole.UserRole>(sql, cancellationToken).ConfigureAwait(false);
        var userRole = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved userRole with id: {Id}.", id);
        return userRole;
    }

    public async Task<Common.UserRole.UserRole?> AddAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userRole);
        _logger.LogDebug("StorageClient: Adding userRole.");
        var sql = await UserRoleDatabaseCommandText.GetInsertSql(userRole).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.UserRole.UserRole>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added userRole.");
        return inserted;
    }

    public async Task<Common.UserRole.UserRole?> UpdateAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userRole);
        _logger.LogDebug("StorageClient: Updating userRole with id: {Id}.", userRole.Id);
        var sql = await UserRoleDatabaseCommandText.GetUpdateSql(userRole).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.UserRole.UserRole>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated userRole with id: {Id}.", userRole.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing userRole with id: {Id}.", id);
        var sql = await UserRoleDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed userRole with id: {Id}.", id);
    }
}
