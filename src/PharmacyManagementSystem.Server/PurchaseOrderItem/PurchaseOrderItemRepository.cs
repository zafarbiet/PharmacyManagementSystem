using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PurchaseOrderItem;

namespace PharmacyManagementSystem.Server.PurchaseOrderItem;

public class PurchaseOrderItemRepository(ILogger<PurchaseOrderItemRepository> logger, IPurchaseOrderItemStorageClient storageClient) : IPurchaseOrderItemRepository
{
    private readonly ILogger<PurchaseOrderItemRepository> _logger = logger;
    private readonly IPurchaseOrderItemStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.PurchaseOrderItem.PurchaseOrderItem>?> GetByFilterCriteriaAsync(PurchaseOrderItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting purchase order items by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} purchase order items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.PurchaseOrderItem.PurchaseOrderItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting purchase order item by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved purchase order item with id: {Id}.", id);

        return result;
    }

    public async Task<Common.PurchaseOrderItem.PurchaseOrderItem?> AddAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrderItem);

        _logger.LogDebug("Repository: Adding purchase order item for PurchaseOrderId: {PurchaseOrderId}.", purchaseOrderItem.PurchaseOrderId);

        var result = await _storageClient.AddAsync(purchaseOrderItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added purchase order item for PurchaseOrderId: {PurchaseOrderId}.", purchaseOrderItem.PurchaseOrderId);

        return result;
    }

    public async Task<Common.PurchaseOrderItem.PurchaseOrderItem?> UpdateAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrderItem);

        _logger.LogDebug("Repository: Updating purchase order item with id: {Id}.", purchaseOrderItem.Id);

        var result = await _storageClient.UpdateAsync(purchaseOrderItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated purchase order item with id: {Id}.", purchaseOrderItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing purchase order item with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed purchase order item with id: {Id}.", id);
    }
}
