using PharmacyManagementSystem.Common.PurchaseOrder;

namespace PharmacyManagementSystem.Server.PurchaseOrder;

public interface IPurchaseOrderRepository
{
    Task<IReadOnlyCollection<Common.PurchaseOrder.PurchaseOrder>?> GetByFilterCriteriaAsync(PurchaseOrderFilter filter, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> AddAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> UpdateAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
