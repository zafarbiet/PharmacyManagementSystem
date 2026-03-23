namespace PharmacyManagementSystem.Server.PurchaseOrder;

public interface ISavePurchaseOrderAction
{
    Task<Common.PurchaseOrder.PurchaseOrder?> AddAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> UpdateAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> ApprovePurchaseOrderAsync(Guid id, string approvedBy, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> RejectPurchaseOrderAsync(Guid id, string rejectedBy, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> VerifyConsignmentAsync(Guid poId, IReadOnlyList<Common.PurchaseOrderItem.PurchaseOrderItem> receivedItems, CancellationToken cancellationToken);
}
