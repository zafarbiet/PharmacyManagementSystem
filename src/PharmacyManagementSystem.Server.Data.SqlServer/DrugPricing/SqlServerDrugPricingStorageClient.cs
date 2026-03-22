using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugPricing;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.DrugPricing;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DrugPricing;

public class SqlServerDrugPricingStorageClient(ILogger<SqlServerDrugPricingStorageClient> logger, ISqlServerDbClient dbClient) : IDrugPricingStorageClient
{
    private readonly ILogger<SqlServerDrugPricingStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DrugPricing.DrugPricing>?> GetByFilterCriteriaAsync(DrugPricingFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting drug pricings by filter criteria.");

        var sql = await DrugPricingDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugPricing.DrugPricing>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} drug pricings.", list.Count);

        return list;
    }

    public async Task<Common.DrugPricing.DrugPricing?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting drug pricing by id: {Id}.", id);

        var sql = await DrugPricingDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugPricing.DrugPricing>(sql, cancellationToken).ConfigureAwait(false);

        var drugPricing = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved drug pricing with id: {Id}.", id);

        return drugPricing;
    }

    public async Task<Common.DrugPricing.DrugPricing?> AddAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);

        _logger.LogDebug("StorageClient: Adding drug pricing for DrugId: {DrugId}.", drugPricing.DrugId);

        var sql = await DrugPricingDatabaseCommandText.GetInsertSql(drugPricing).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugPricing.DrugPricing>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added drug pricing for DrugId: {DrugId}.", drugPricing.DrugId);

        return inserted;
    }

    public async Task<Common.DrugPricing.DrugPricing?> UpdateAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);

        _logger.LogDebug("StorageClient: Updating drug pricing with id: {Id}.", drugPricing.Id);

        var sql = await DrugPricingDatabaseCommandText.GetUpdateSql(drugPricing).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugPricing.DrugPricing>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated drug pricing with id: {Id}.", drugPricing.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing drug pricing with id: {Id}.", id);

        var sql = await DrugPricingDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed drug pricing with id: {Id}.", id);
    }
}
