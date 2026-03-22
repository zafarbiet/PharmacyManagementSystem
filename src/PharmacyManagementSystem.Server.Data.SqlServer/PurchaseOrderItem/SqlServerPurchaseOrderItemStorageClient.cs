using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PurchaseOrderItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.PurchaseOrderItem;

namespace PharmacyManagementSystem.Server.Data.SqlServer.PurchaseOrderItem;

public class SqlServerPurchaseOrderItemStorageClient(ILogger<SqlServerPurchaseOrderItemStorageClient> logger, ISqlServerDbClient dbClient) : IPurchaseOrderItemStorageClient
{
    private readonly ILogger<SqlServerPurchaseOrderItemStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.PurchaseOrderItem.PurchaseOrderItem>?> GetByFilterCriteriaAsync(PurchaseOrderItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting purchase order items by filter criteria.");

        var sql = await PurchaseOrderItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrderItem.PurchaseOrderItem>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} purchase order items.", list.Count);

        return list;
    }

    public async Task<Common.PurchaseOrderItem.PurchaseOrderItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting purchase order item by id: {Id}.", id);

        var sql = await PurchaseOrderItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrderItem.PurchaseOrderItem>(sql, cancellationToken).ConfigureAwait(false);

        var purchaseOrderItem = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved purchase order item with id: {Id}.", id);

        return purchaseOrderItem;
    }

    public async Task<Common.PurchaseOrderItem.PurchaseOrderItem?> AddAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrderItem);

        _logger.LogDebug("StorageClient: Adding purchase order item for PurchaseOrderId: {PurchaseOrderId}.", purchaseOrderItem.PurchaseOrderId);

        var sql = await PurchaseOrderItemDatabaseCommandText.GetInsertSql(purchaseOrderItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrderItem.PurchaseOrderItem>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added purchase order item for PurchaseOrderId: {PurchaseOrderId}.", purchaseOrderItem.PurchaseOrderId);

        return inserted;
    }

    public async Task<Common.PurchaseOrderItem.PurchaseOrderItem?> UpdateAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrderItem);

        _logger.LogDebug("StorageClient: Updating purchase order item with id: {Id}.", purchaseOrderItem.Id);

        var sql = await PurchaseOrderItemDatabaseCommandText.GetUpdateSql(purchaseOrderItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrderItem.PurchaseOrderItem>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated purchase order item with id: {Id}.", purchaseOrderItem.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing purchase order item with id: {Id}.", id);

        var sql = await PurchaseOrderItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed purchase order item with id: {Id}.", id);
    }
}
