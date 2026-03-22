using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.StorageZone;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.StorageZone;

namespace PharmacyManagementSystem.Server.Data.SqlServer.StorageZone;

public class SqlServerStorageZoneStorageClient(ILogger<SqlServerStorageZoneStorageClient> logger, ISqlServerDbClient dbClient) : IStorageZoneStorageClient
{
    private readonly ILogger<SqlServerStorageZoneStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.StorageZone.StorageZone>?> GetByFilterCriteriaAsync(StorageZoneFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting storage zones by filter criteria.");

        var sql = await StorageZoneDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StorageZone.StorageZone>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} storage zones.", list.Count);

        return list;
    }

    public async Task<Common.StorageZone.StorageZone?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting storage zone by id: {Id}.", id);

        var sql = await StorageZoneDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StorageZone.StorageZone>(sql, cancellationToken).ConfigureAwait(false);

        var storageZone = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved storage zone with id: {Id}.", id);

        return storageZone;
    }

    public async Task<Common.StorageZone.StorageZone?> AddAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(storageZone);

        _logger.LogDebug("StorageClient: Adding storage zone with name: {Name}.", storageZone.Name);

        var sql = await StorageZoneDatabaseCommandText.GetInsertSql(storageZone).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StorageZone.StorageZone>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added storage zone with name: {Name}.", storageZone.Name);

        return inserted;
    }

    public async Task<Common.StorageZone.StorageZone?> UpdateAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(storageZone);

        _logger.LogDebug("StorageClient: Updating storage zone with id: {Id}.", storageZone.Id);

        var sql = await StorageZoneDatabaseCommandText.GetUpdateSql(storageZone).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StorageZone.StorageZone>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated storage zone with id: {Id}.", storageZone.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing storage zone with id: {Id}.", id);

        var sql = await StorageZoneDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed storage zone with id: {Id}.", id);
    }
}
