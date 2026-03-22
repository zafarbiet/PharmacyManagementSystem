using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventory;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.DrugInventory;

namespace PharmacyManagementSystem.Server.Data.SqlServer.DrugInventory;

public class SqlServerDrugInventoryStorageClient(ILogger<SqlServerDrugInventoryStorageClient> logger, ISqlServerDbClient dbClient) : IDrugInventoryStorageClient
{
    private readonly ILogger<SqlServerDrugInventoryStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DrugInventory.DrugInventory>?> GetByFilterCriteriaAsync(DrugInventoryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting drug inventories by filter criteria.");

        var sql = await DrugInventoryDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventory.DrugInventory>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} drug inventories.", list.Count);

        return list;
    }

    public async Task<Common.DrugInventory.DrugInventory?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting drug inventory by id: {Id}.", id);

        var sql = await DrugInventoryDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventory.DrugInventory>(sql, cancellationToken).ConfigureAwait(false);

        var drugInventory = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved drug inventory with id: {Id}.", id);

        return drugInventory;
    }

    public async Task<Common.DrugInventory.DrugInventory?> AddAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        _logger.LogDebug("StorageClient: Adding drug inventory for DrugId: {DrugId}.", drugInventory.DrugId);

        var sql = await DrugInventoryDatabaseCommandText.GetInsertSql(drugInventory).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventory.DrugInventory>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added drug inventory for DrugId: {DrugId}.", drugInventory.DrugId);

        return inserted;
    }

    public async Task<Common.DrugInventory.DrugInventory?> UpdateAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        _logger.LogDebug("StorageClient: Updating drug inventory with id: {Id}.", drugInventory.Id);

        var sql = await DrugInventoryDatabaseCommandText.GetUpdateSql(drugInventory).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DrugInventory.DrugInventory>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated drug inventory with id: {Id}.", drugInventory.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing drug inventory with id: {Id}.", id);

        var sql = await DrugInventoryDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed drug inventory with id: {Id}.", id);
    }
}
