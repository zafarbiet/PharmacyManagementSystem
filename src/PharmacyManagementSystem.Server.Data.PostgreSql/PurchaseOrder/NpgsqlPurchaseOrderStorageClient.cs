using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PurchaseOrder;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.PurchaseOrder;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.PurchaseOrder;

public class NpgsqlPurchaseOrderStorageClient(ILogger<NpgsqlPurchaseOrderStorageClient> logger, INpgsqlDbClient dbClient) : IPurchaseOrderStorageClient
{
    private readonly ILogger<NpgsqlPurchaseOrderStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.PurchaseOrder.PurchaseOrder?>?> GetByFilterCriteriaAsync(PurchaseOrderFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting purchaseOrders by filter criteria.");
        var sql = await PurchaseOrderDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrder.PurchaseOrder>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} purchaseOrders.", list.Count);
        return list;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting purchaseOrder by id: {Id}.", id);
        var sql = await PurchaseOrderDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrder.PurchaseOrder>(sql, cancellationToken).ConfigureAwait(false);
        var purchaseOrder = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved purchaseOrder with id: {Id}.", id);
        return purchaseOrder;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> AddAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);
        _logger.LogDebug("StorageClient: Adding purchaseOrder.");
        var sql = await PurchaseOrderDatabaseCommandText.GetInsertSql(purchaseOrder).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrder.PurchaseOrder>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added purchaseOrder.");
        return inserted;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> UpdateAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);
        _logger.LogDebug("StorageClient: Updating purchaseOrder with id: {Id}.", purchaseOrder.Id);
        var sql = await PurchaseOrderDatabaseCommandText.GetUpdateSql(purchaseOrder).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrder.PurchaseOrder>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated purchaseOrder with id: {Id}.", purchaseOrder.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing purchaseOrder with id: {Id}.", id);
        var sql = await PurchaseOrderDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed purchaseOrder with id: {Id}.", id);
    }
}
