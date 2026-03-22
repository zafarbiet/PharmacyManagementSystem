using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.PurchaseOrder;

public class SavePurchaseOrderAction(ILogger<SavePurchaseOrderAction> logger, IPurchaseOrderRepository repository) : ISavePurchaseOrderAction
{
    private readonly ILogger<SavePurchaseOrderAction> _logger = logger;
    private readonly IPurchaseOrderRepository _repository = repository;

    public async Task<Common.PurchaseOrder.PurchaseOrder?> AddAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);

        if (purchaseOrder.VendorId == Guid.Empty)
            throw new BadRequestException("PurchaseOrder VendorId is required.");

        if (string.IsNullOrWhiteSpace(purchaseOrder.Status))
            throw new BadRequestException("PurchaseOrder Status is required.");

        purchaseOrder.UpdatedBy = "system";

        _logger.LogDebug("Adding new purchase order for VendorId: {VendorId}.", purchaseOrder.VendorId);

        var result = await _repository.AddAsync(purchaseOrder, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added purchase order for VendorId: {VendorId}.", purchaseOrder.VendorId);

        return result;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> UpdateAsync(Common.PurchaseOrder.PurchaseOrder? purchaseOrder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);

        if (purchaseOrder.VendorId == Guid.Empty)
            throw new BadRequestException("PurchaseOrder VendorId is required.");

        if (string.IsNullOrWhiteSpace(purchaseOrder.Status))
            throw new BadRequestException("PurchaseOrder Status is required.");

        purchaseOrder.UpdatedBy = "system";

        _logger.LogDebug("Updating purchase order with id: {Id}.", purchaseOrder.Id);

        var result = await _repository.UpdateAsync(purchaseOrder, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated purchase order with id: {Id}.", purchaseOrder.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing purchase order with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed purchase order with id: {Id}.", id);
    }
}
