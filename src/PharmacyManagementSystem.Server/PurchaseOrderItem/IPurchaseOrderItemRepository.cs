using PharmacyManagementSystem.Common.PurchaseOrderItem;

namespace PharmacyManagementSystem.Server.PurchaseOrderItem;

public interface IPurchaseOrderItemRepository
{
    Task<IReadOnlyCollection<Common.PurchaseOrderItem.PurchaseOrderItem>?> GetByFilterCriteriaAsync(PurchaseOrderItemFilter filter, CancellationToken cancellationToken);
    Task<Common.PurchaseOrderItem.PurchaseOrderItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.PurchaseOrderItem.PurchaseOrderItem?> AddAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken);
    Task<Common.PurchaseOrderItem.PurchaseOrderItem?> UpdateAsync(Common.PurchaseOrderItem.PurchaseOrderItem? purchaseOrderItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
