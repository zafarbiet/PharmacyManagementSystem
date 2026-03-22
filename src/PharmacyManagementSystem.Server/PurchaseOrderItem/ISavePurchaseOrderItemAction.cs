namespace PharmacyManagementSystem.Server.PurchaseOrderItem;

public interface ISavePurchaseOrderItemAction
{
    Task<Common.PurchaseOrderItem.PurchaseOrderItem?> AddAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken);
    Task<Common.PurchaseOrderItem.PurchaseOrderItem?> UpdateAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
