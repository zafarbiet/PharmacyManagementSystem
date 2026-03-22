namespace PharmacyManagementSystem.Server.PurchaseOrder;

public interface ISavePurchaseOrderAction
{
    Task<Common.PurchaseOrder.PurchaseOrder?> AddAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> UpdateAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
