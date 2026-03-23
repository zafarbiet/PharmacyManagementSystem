using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Server.Notification;
using PharmacyManagementSystem.Server.PurchaseOrderItem;

namespace PharmacyManagementSystem.Server.PurchaseOrder;

public class SavePurchaseOrderAction(
    ILogger<SavePurchaseOrderAction> logger,
    IPurchaseOrderRepository repository,
    IPurchaseOrderItemRepository purchaseOrderItemRepository,
    ISaveNotificationAction notificationAction) : ISavePurchaseOrderAction
{
    private readonly ILogger<SavePurchaseOrderAction> _logger = logger;
    private readonly IPurchaseOrderRepository _repository = repository;
    private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository = purchaseOrderItemRepository;
    private readonly ISaveNotificationAction _notificationAction = notificationAction;

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

    public async Task<Common.PurchaseOrder.PurchaseOrder?> ApprovePurchaseOrderAsync(Guid id, string approvedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(approvedBy);

        _logger.LogDebug("Approving purchase order {Id} by {ApprovedBy}.", id, approvedBy);

        var po = await _repository.GetByIdAsync(id.ToString(), cancellationToken).ConfigureAwait(false);
        if (po is null)
            throw new BadRequestException($"Purchase order {id} not found.");

        if (po.Status is "Approved" or "Received")
            throw new ConflictException($"Purchase order {id} is already in status '{po.Status}' and cannot be approved.");

        po.Status = "Approved";
        po.ApprovedBy = approvedBy;
        po.ApprovedAt = DateTimeOffset.UtcNow;
        po.UpdatedBy = approvedBy;

        var result = await _repository.UpdateAsync(po, cancellationToken).ConfigureAwait(false);

        // eWay bill stub: notify if total exceeds ₹50,000 (may be interstate)
        if (po.TotalAmount > 50_000m)
        {
            await _notificationAction.AddAsync(new Common.Notification.Notification
            {
                NotificationType = "EWayBillReminder",
                Channel = "InApp",
                RecipientType = "Staff",
                RecipientId = Guid.Empty,
                Subject = "eWay Bill May Be Required",
                Body = $"Purchase Order {id} has been approved with total amount ₹{po.TotalAmount:N2} which exceeds ₹50,000. If this is an interstate transaction, an eWay bill is required under GST rules.",
                ReferenceId = id,
                ReferenceType = "PurchaseOrder",
                ScheduledAt = DateTimeOffset.UtcNow,
                Status = "Pending",
                UpdatedBy = approvedBy
            }, cancellationToken).ConfigureAwait(false);
        }

        _logger.LogDebug("Approved purchase order {Id}.", id);

        return result;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> RejectPurchaseOrderAsync(Guid id, string rejectedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rejectedBy);

        _logger.LogDebug("Rejecting purchase order {Id} by {RejectedBy}.", id, rejectedBy);

        var po = await _repository.GetByIdAsync(id.ToString(), cancellationToken).ConfigureAwait(false);
        if (po is null)
            throw new BadRequestException($"Purchase order {id} not found.");

        if (po.Status is "Approved" or "Received")
            throw new ConflictException($"Purchase order {id} is in status '{po.Status}' and cannot be rejected.");

        po.Status = "Rejected";
        po.UpdatedBy = rejectedBy;

        var result = await _repository.UpdateAsync(po, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Rejected purchase order {Id}.", id);

        return result;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> VerifyConsignmentAsync(
        Guid poId,
        IReadOnlyList<Common.PurchaseOrderItem.PurchaseOrderItem> receivedItems,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(receivedItems);

        _logger.LogDebug("Verifying consignment for purchase order {PoId}.", poId);

        var po = await _repository.GetByIdAsync(poId.ToString(), cancellationToken).ConfigureAwait(false);
        if (po is null)
            throw new BadRequestException($"Purchase order {poId} not found.");

        if (po.Status is not ("Approved" or "PartiallyReceived"))
            throw new ConflictException($"Purchase order {poId} must be Approved or PartiallyReceived to verify consignment. Current status: '{po.Status}'.");

        var originalItems = await _purchaseOrderItemRepository.GetByFilterCriteriaAsync(
            new Common.PurchaseOrderItem.PurchaseOrderItemFilter { PurchaseOrderId = poId },
            cancellationToken).ConfigureAwait(false);

        if (originalItems is null || originalItems.Count == 0)
            throw new BadRequestException($"No items found for purchase order {poId}.");

        var shortfallItems = new List<Common.PurchaseOrderItem.PurchaseOrderItem>();
        var allFulfilled = true;

        foreach (var original in originalItems)
        {
            var received = receivedItems.FirstOrDefault(r => r.DrugId == original.DrugId);
            var receivedQty = received?.QuantityReceived ?? 0;

            original.QuantityReceived = receivedQty;
            original.UpdatedBy = "system";
            await _purchaseOrderItemRepository.UpdateAsync(original, cancellationToken).ConfigureAwait(false);

            var remaining = original.QuantityOrdered - receivedQty;
            if (remaining > 0)
            {
                allFulfilled = false;
                shortfallItems.Add(new Common.PurchaseOrderItem.PurchaseOrderItem
                {
                    DrugId = original.DrugId,
                    QuantityOrdered = remaining,
                    QuantityReceived = 0,
                    UnitPrice = original.UnitPrice
                });
            }
        }

        if (allFulfilled)
        {
            po.Status = "Received";
        }
        else
        {
            po.Status = "PartiallyReceived";

            // Create child PO for the shortfall
            var childPo = new Common.PurchaseOrder.PurchaseOrder
            {
                VendorId = po.VendorId,
                OrderDate = DateTimeOffset.UtcNow,
                Status = "Pending",
                Notes = $"Shortfall child PO of {poId}",
                ParentPurchaseOrderId = poId,
                BranchId = po.BranchId,
                UpdatedBy = "system"
            };

            var createdChildPo = await _repository.AddAsync(childPo, cancellationToken).ConfigureAwait(false);

            if (createdChildPo is not null)
            {
                foreach (var shortfall in shortfallItems)
                {
                    shortfall.PurchaseOrderId = createdChildPo.Id;
                    shortfall.UpdatedBy = "system";
                    await _purchaseOrderItemRepository.AddAsync(shortfall, cancellationToken).ConfigureAwait(false);
                }

                _logger.LogDebug("Created child PO {ChildPoId} for {Count} shortfall items.", createdChildPo.Id, shortfallItems.Count);
            }
        }

        po.UpdatedBy = "system";
        var result = await _repository.UpdateAsync(po, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Consignment verified for PO {PoId}. Status: {Status}.", poId, po.Status);

        return result;
    }
}
