using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PurchaseOrder;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.PurchaseOrder;

namespace PharmacyManagementSystem.Server.Data.SqlServer.PurchaseOrder;

public class SqlServerPurchaseOrderStorageClient(ILogger<SqlServerPurchaseOrderStorageClient> logger, ISqlServerDbClient dbClient) : IPurchaseOrderStorageClient
{
    private readonly ILogger<SqlServerPurchaseOrderStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.PurchaseOrder.PurchaseOrder>?> GetByFilterCriteriaAsync(PurchaseOrderFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting purchase orders by filter criteria.");

        var sql = await PurchaseOrderDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrder.PurchaseOrder>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} purchase orders.", list.Count);

        return list;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting purchase order by id: {Id}.", id);

        var sql = await PurchaseOrderDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrder.PurchaseOrder>(sql, cancellationToken).ConfigureAwait(false);

        var purchaseOrder = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved purchase order with id: {Id}.", id);

        return purchaseOrder;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> AddAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);

        _logger.LogDebug("StorageClient: Adding purchase order for VendorId: {VendorId}.", purchaseOrder.VendorId);

        var sql = await PurchaseOrderDatabaseCommandText.GetInsertSql(purchaseOrder).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrder.PurchaseOrder>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added purchase order for VendorId: {VendorId}.", purchaseOrder.VendorId);

        return inserted;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> UpdateAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);

        _logger.LogDebug("StorageClient: Updating purchase order with id: {Id}.", purchaseOrder.Id);

        var sql = await PurchaseOrderDatabaseCommandText.GetUpdateSql(purchaseOrder).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PurchaseOrder.PurchaseOrder>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated purchase order with id: {Id}.", purchaseOrder.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing purchase order with id: {Id}.", id);

        var sql = await PurchaseOrderDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed purchase order with id: {Id}.", id);
    }
}
