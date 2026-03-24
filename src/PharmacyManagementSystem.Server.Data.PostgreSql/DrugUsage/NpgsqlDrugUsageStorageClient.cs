using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugUsage;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DrugUsage;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DrugUsage;

public class NpgsqlDrugUsageStorageClient(ILogger<NpgsqlDrugUsageStorageClient> logger, INpgsqlDbClient dbClient) : IDrugUsageStorageClient
{
    private readonly ILogger<NpgsqlDrugUsageStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DrugUsage.DrugUsage?>?> GetByFilterCriteriaAsync(DrugUsageFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting drugUsages by filter criteria.");
        var sql = await DrugUsageDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugUsage.DrugUsage>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} drugUsages.", list.Count);
        return list;
    }

    public async Task<Common.DrugUsage.DrugUsage?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting drugUsage by id: {Id}.", id);
        var sql = await DrugUsageDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugUsage.DrugUsage>(sql, cancellationToken).ConfigureAwait(false);
        var drugUsage = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved drugUsage with id: {Id}.", id);
        return drugUsage;
    }

    public async Task<Common.DrugUsage.DrugUsage?> AddAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);
        _logger.LogDebug("StorageClient: Adding drugUsage.");
        var sql = await DrugUsageDatabaseCommandText.GetInsertSql(drugUsage).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugUsage.DrugUsage>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added drugUsage.");
        return inserted;
    }

    public async Task<Common.DrugUsage.DrugUsage?> UpdateAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);
        _logger.LogDebug("StorageClient: Updating drugUsage with id: {Id}.", drugUsage.Id);
        var sql = await DrugUsageDatabaseCommandText.GetUpdateSql(drugUsage).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugUsage.DrugUsage>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated drugUsage with id: {Id}.", drugUsage.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing drugUsage with id: {Id}.", id);
        var sql = await DrugUsageDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed drugUsage with id: {Id}.", id);
    }
}
