using PharmacyManagementSystem.Common.PurchaseOrder;

namespace PharmacyManagementSystem.Server.PurchaseOrder;

public interface IGetPurchaseOrderAction
{
    Task<IReadOnlyCollection<Common.PurchaseOrder.PurchaseOrder>?> GetByFilterCriteriaAsync(PurchaseOrderFilter filter, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
