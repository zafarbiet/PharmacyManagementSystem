using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.AppUser;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.AppUser;

namespace PharmacyManagementSystem.Server.Data.SqlServer.AppUser;

public class SqlServerAppUserStorageClient(ILogger<SqlServerAppUserStorageClient> logger, ISqlServerDbClient dbClient) : IAppUserStorageClient
{
    private readonly ILogger<SqlServerAppUserStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.AppUser.AppUser>?> GetByFilterCriteriaAsync(AppUserFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting app users by filter criteria.");

        var sql = await AppUserDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.AppUser.AppUser>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} app users.", list.Count);

        return list;
    }

    public async Task<Common.AppUser.AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting app user by id: {Id}.", id);

        var sql = await AppUserDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.AppUser.AppUser>(sql, cancellationToken).ConfigureAwait(false);

        var appUser = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved app user with id: {Id}.", id);

        return appUser;
    }

    public async Task<Common.AppUser.AppUser?> AddAsync(Common.AppUser.AppUser? appUser, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(appUser);

        _logger.LogDebug("StorageClient: Adding app user with username: {Username}.", appUser.Username);

        var sql = await AppUserDatabaseCommandText.GetInsertSql(appUser).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.AppUser.AppUser>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added app user with username: {Username}.", appUser.Username);

        return inserted;
    }

    public async Task<Common.AppUser.AppUser?> UpdateAsync(Common.AppUser.AppUser? appUser, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(appUser);

        _logger.LogDebug("StorageClient: Updating app user with id: {Id}.", appUser.Id);

        var sql = await AppUserDatabaseCommandText.GetUpdateSql(appUser).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.AppUser.AppUser>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated app user with id: {Id}.", appUser.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing app user with id: {Id}.", id);

        var sql = await AppUserDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed app user with id: {Id}.", id);
    }
}
