using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventory;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DrugInventory;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DrugInventory;

public class NpgsqlDrugInventoryStorageClient(ILogger<NpgsqlDrugInventoryStorageClient> logger, INpgsqlDbClient dbClient) : IDrugInventoryStorageClient
{
    private readonly ILogger<NpgsqlDrugInventoryStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DrugInventory.DrugInventory?>?> GetByFilterCriteriaAsync(DrugInventoryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting drugInventorys by filter criteria.");
        var sql = await DrugInventoryDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventory.DrugInventory>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} drugInventorys.", list.Count);
        return list;
    }

    public async Task<Common.DrugInventory.DrugInventory?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting drugInventory by id: {Id}.", id);
        var sql = await DrugInventoryDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventory.DrugInventory>(sql, cancellationToken).ConfigureAwait(false);
        var drugInventory = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved drugInventory with id: {Id}.", id);
        return drugInventory;
    }

    public async Task<Common.DrugInventory.DrugInventory?> AddAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);
        _logger.LogDebug("StorageClient: Adding drugInventory.");
        var sql = await DrugInventoryDatabaseCommandText.GetInsertSql(drugInventory).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventory.DrugInventory>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added drugInventory.");
        return inserted;
    }

    public async Task<Common.DrugInventory.DrugInventory?> UpdateAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);
        _logger.LogDebug("StorageClient: Updating drugInventory with id: {Id}.", drugInventory.Id);
        var sql = await DrugInventoryDatabaseCommandText.GetUpdateSql(drugInventory).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventory.DrugInventory>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated drugInventory with id: {Id}.", drugInventory.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing drugInventory with id: {Id}.", id);
        var sql = await DrugInventoryDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed drugInventory with id: {Id}.", id);
    }
}
