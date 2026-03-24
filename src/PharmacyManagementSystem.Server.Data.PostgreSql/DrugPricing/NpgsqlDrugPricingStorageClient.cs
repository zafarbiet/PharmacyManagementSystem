using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugPricing;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DrugPricing;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DrugPricing;

public class NpgsqlDrugPricingStorageClient(ILogger<NpgsqlDrugPricingStorageClient> logger, INpgsqlDbClient dbClient) : IDrugPricingStorageClient
{
    private readonly ILogger<NpgsqlDrugPricingStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DrugPricing.DrugPricing?>?> GetByFilterCriteriaAsync(DrugPricingFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting drugPricings by filter criteria.");
        var sql = await DrugPricingDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugPricing.DrugPricing>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} drugPricings.", list.Count);
        return list;
    }

    public async Task<Common.DrugPricing.DrugPricing?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting drugPricing by id: {Id}.", id);
        var sql = await DrugPricingDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugPricing.DrugPricing>(sql, cancellationToken).ConfigureAwait(false);
        var drugPricing = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved drugPricing with id: {Id}.", id);
        return drugPricing;
    }

    public async Task<Common.DrugPricing.DrugPricing?> AddAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);
        _logger.LogDebug("StorageClient: Adding drugPricing.");
        var sql = await DrugPricingDatabaseCommandText.GetInsertSql(drugPricing).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugPricing.DrugPricing>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added drugPricing.");
        return inserted;
    }

    public async Task<Common.DrugPricing.DrugPricing?> UpdateAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);
        _logger.LogDebug("StorageClient: Updating drugPricing with id: {Id}.", drugPricing.Id);
        var sql = await DrugPricingDatabaseCommandText.GetUpdateSql(drugPricing).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugPricing.DrugPricing>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated drugPricing with id: {Id}.", drugPricing.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing drugPricing with id: {Id}.", id);
        var sql = await DrugPricingDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed drugPricing with id: {Id}.", id);
    }
}
