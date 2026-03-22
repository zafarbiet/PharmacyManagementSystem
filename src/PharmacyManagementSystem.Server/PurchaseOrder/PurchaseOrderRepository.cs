using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PurchaseOrder;

namespace PharmacyManagementSystem.Server.PurchaseOrder;

public class PurchaseOrderRepository(ILogger<PurchaseOrderRepository> logger, IPurchaseOrderStorageClient storageClient) : IPurchaseOrderRepository
{
    private readonly ILogger<PurchaseOrderRepository> _logger = logger;
    private readonly IPurchaseOrderStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.PurchaseOrder.PurchaseOrder>?> GetByFilterCriteriaAsync(PurchaseOrderFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting purchase orders by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} purchase orders.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting purchase order by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved purchase order with id: {Id}.", id);

        return result;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> AddAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);

        _logger.LogDebug("Repository: Adding purchase order for VendorId: {VendorId}.", purchaseOrder.VendorId);

        var result = await _storageClient.AddAsync(purchaseOrder, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added purchase order for VendorId: {VendorId}.", purchaseOrder.VendorId);

        return result;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> UpdateAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);

        _logger.LogDebug("Repository: Updating purchase order with id: {Id}.", purchaseOrder.Id);

        var result = await _storageClient.UpdateAsync(purchaseOrder, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated purchase order with id: {Id}.", purchaseOrder.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing purchase order with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed purchase order with id: {Id}.", id);
    }
}
