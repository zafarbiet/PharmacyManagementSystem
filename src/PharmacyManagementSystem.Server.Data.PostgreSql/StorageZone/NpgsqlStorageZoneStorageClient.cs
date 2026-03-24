using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.StorageZone;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.StorageZone;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.StorageZone;

public class NpgsqlStorageZoneStorageClient(ILogger<NpgsqlStorageZoneStorageClient> logger, INpgsqlDbClient dbClient) : IStorageZoneStorageClient
{
    private readonly ILogger<NpgsqlStorageZoneStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.StorageZone.StorageZone?>?> GetByFilterCriteriaAsync(StorageZoneFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting storageZones by filter criteria.");
        var sql = await StorageZoneDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StorageZone.StorageZone>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} storageZones.", list.Count);
        return list;
    }

    public async Task<Common.StorageZone.StorageZone?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting storageZone by id: {Id}.", id);
        var sql = await StorageZoneDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StorageZone.StorageZone>(sql, cancellationToken).ConfigureAwait(false);
        var storageZone = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved storageZone with id: {Id}.", id);
        return storageZone;
    }

    public async Task<Common.StorageZone.StorageZone?> AddAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(storageZone);
        _logger.LogDebug("StorageClient: Adding storageZone.");
        var sql = await StorageZoneDatabaseCommandText.GetInsertSql(storageZone).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StorageZone.StorageZone>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added storageZone.");
        return inserted;
    }

    public async Task<Common.StorageZone.StorageZone?> UpdateAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(storageZone);
        _logger.LogDebug("StorageClient: Updating storageZone with id: {Id}.", storageZone.Id);
        var sql = await StorageZoneDatabaseCommandText.GetUpdateSql(storageZone).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.StorageZone.StorageZone>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated storageZone with id: {Id}.", storageZone.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing storageZone with id: {Id}.", id);
        var sql = await StorageZoneDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed storageZone with id: {Id}.", id);
    }
}
