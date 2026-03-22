using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugUsage;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.DrugUsage;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DrugUsage;

public class SqlServerDrugUsageStorageClient(ILogger<SqlServerDrugUsageStorageClient> logger, ISqlServerDbClient dbClient) : IDrugUsageStorageClient
{
    private readonly ILogger<SqlServerDrugUsageStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DrugUsage.DrugUsage>?> GetByFilterCriteriaAsync(DrugUsageFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting drug usages by filter criteria.");

        var sql = await DrugUsageDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugUsage.DrugUsage>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} drug usages.", list.Count);

        return list;
    }

    public async Task<Common.DrugUsage.DrugUsage?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting drug usage by id: {Id}.", id);

        var sql = await DrugUsageDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugUsage.DrugUsage>(sql, cancellationToken).ConfigureAwait(false);

        var drugUsage = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved drug usage with id: {Id}.", id);

        return drugUsage;
    }

    public async Task<Common.DrugUsage.DrugUsage?> AddAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);

        _logger.LogDebug("StorageClient: Adding drug usage for DrugId: {DrugId}.", drugUsage.DrugId);

        var sql = await DrugUsageDatabaseCommandText.GetInsertSql(drugUsage).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugUsage.DrugUsage>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added drug usage for DrugId: {DrugId}.", drugUsage.DrugId);

        return inserted;
    }

    public async Task<Common.DrugUsage.DrugUsage?> UpdateAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);

        _logger.LogDebug("StorageClient: Updating drug usage with id: {Id}.", drugUsage.Id);

        var sql = await DrugUsageDatabaseCommandText.GetUpdateSql(drugUsage).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugUsage.DrugUsage>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated drug usage with id: {Id}.", drugUsage.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing drug usage with id: {Id}.", id);

        var sql = await DrugUsageDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed drug usage with id: {Id}.", id);
    }
}
