using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.PurchaseOrderItem;

public class SavePurchaseOrderItemAction(ILogger<SavePurchaseOrderItemAction> logger, IPurchaseOrderItemRepository repository) : ISavePurchaseOrderItemAction
{
    private readonly ILogger<SavePurchaseOrderItemAction> _logger = logger;
    private readonly IPurchaseOrderItemRepository _repository = repository;

    public async Task<Common.PurchaseOrderItem.PurchaseOrderItem?> AddAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrderItem);

        if (purchaseOrderItem.PurchaseOrderId == Guid.Empty)
            throw new BadRequestException("PurchaseOrderItem PurchaseOrderId is required.");

        if (purchaseOrderItem.DrugId == Guid.Empty)
            throw new BadRequestException("PurchaseOrderItem DrugId is required.");

        if (purchaseOrderItem.QuantityOrdered <= 0)
            throw new BadRequestException("PurchaseOrderItem QuantityOrdered must be > 0.");

        purchaseOrderItem.UpdatedBy = "system";

        _logger.LogDebug("Adding new purchase order item for PurchaseOrderId: {PurchaseOrderId}.", purchaseOrderItem.PurchaseOrderId);

        var result = await _repository.AddAsync(purchaseOrderItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added purchase order item for PurchaseOrderId: {PurchaseOrderId}.", purchaseOrderItem.PurchaseOrderId);

        return result;
    }

    public async Task<Common.PurchaseOrderItem.PurchaseOrderItem?> UpdateAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrderItem);

        if (purchaseOrderItem.PurchaseOrderId == Guid.Empty)
            throw new BadRequestException("PurchaseOrderItem PurchaseOrderId is required.");

        if (purchaseOrderItem.DrugId == Guid.Empty)
            throw new BadRequestException("PurchaseOrderItem DrugId is required.");

        if (purchaseOrderItem.QuantityOrdered <= 0)
            throw new BadRequestException("PurchaseOrderItem QuantityOrdered must be > 0.");

        purchaseOrderItem.UpdatedBy = "system";

        _logger.LogDebug("Updating purchase order item with id: {Id}.", purchaseOrderItem.Id);

        var result = await _repository.UpdateAsync(purchaseOrderItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated purchase order item with id: {Id}.", purchaseOrderItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing purchase order item with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed purchase order item with id: {Id}.", id);
    }
}
