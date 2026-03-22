using PharmacyManagementSystem.Common.PurchaseOrderItem;

namespace PharmacyManagementSystem.Server.PurchaseOrderItem;

public interface IGetPurchaseOrderItemAction
{
    Task<IReadOnlyCollection<Common.PurchaseOrderItem.PurchaseOrderItem>?> GetByFilterCriteriaAsync(PurchaseOrderItemFilter filter, CancellationToken cancellationToken);
    Task<Common.PurchaseOrderItem.PurchaseOrderItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
