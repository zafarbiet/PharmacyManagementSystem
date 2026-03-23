using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;
using PharmacyManagementSystem.Common.Quotation;
using PharmacyManagementSystem.Server.PurchaseOrder;
using PharmacyManagementSystem.Server.PurchaseOrderItem;
using PharmacyManagementSystem.Server.QuotationItem;

namespace PharmacyManagementSystem.Server.Quotation;

public class SaveQuotationAction(
    ILogger<SaveQuotationAction> logger,
    IQuotationRepository repository,
    IQuotationItemRepository quotationItemRepository,
    IPurchaseOrderRepository purchaseOrderRepository,
    IPurchaseOrderItemRepository purchaseOrderItemRepository) : ISaveQuotationAction
{
    private readonly ILogger<SaveQuotationAction> _logger = logger;
    private readonly IQuotationRepository _repository = repository;
    private readonly IQuotationItemRepository _quotationItemRepository = quotationItemRepository;
    private readonly IPurchaseOrderRepository _purchaseOrderRepository = purchaseOrderRepository;
    private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository = purchaseOrderItemRepository;

    public async Task<Common.Quotation.Quotation?> AddAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        if (quotation.QuotationRequestId == Guid.Empty)
            throw new BadRequestException("Quotation QuotationRequestId is required.");

        if (quotation.VendorId == Guid.Empty)
            throw new BadRequestException("Quotation VendorId is required.");

        if (string.IsNullOrWhiteSpace(quotation.Status))
            throw new BadRequestException("Quotation Status is required.");

        quotation.UpdatedBy = "system";

        _logger.LogDebug("Adding new quotation.");

        var result = await _repository.AddAsync(quotation, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added quotation.");

        return result;
    }

    public async Task<Common.Quotation.Quotation?> UpdateAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        if (quotation.QuotationRequestId == Guid.Empty)
            throw new BadRequestException("Quotation QuotationRequestId is required.");

        if (quotation.VendorId == Guid.Empty)
            throw new BadRequestException("Quotation VendorId is required.");

        if (string.IsNullOrWhiteSpace(quotation.Status))
            throw new BadRequestException("Quotation Status is required.");

        quotation.UpdatedBy = "system";

        _logger.LogDebug("Updating quotation with id: {Id}.", quotation.Id);

        var result = await _repository.UpdateAsync(quotation, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated quotation with id: {Id}.", quotation.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing quotation with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed quotation with id: {Id}.", id);
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> AcceptAsync(Guid quotationId, Guid? branchId, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Accepting quotation {QuotationId}.", quotationId);

        var quotation = await _repository.GetByIdAsync(quotationId.ToString(), cancellationToken).ConfigureAwait(false);
        if (quotation is null)
            throw new BadRequestException($"Quotation {quotationId} not found.");

        if (quotation.Status is "Accepted")
            throw new ConflictException($"Quotation {quotationId} is already accepted.");

        // Reject all other quotations for the same RFQ
        var competing = await _repository.GetByFilterCriteriaAsync(
            new QuotationFilter { QuotationRequestId = quotation.QuotationRequestId },
            cancellationToken).ConfigureAwait(false);

        foreach (var other in competing ?? [])
        {
            if (other.Id == quotationId || other.Status is "Rejected")
                continue;

            other.Status = "Rejected";
            other.UpdatedBy = "system";
            await _repository.UpdateAsync(other, cancellationToken).ConfigureAwait(false);
        }

        // Accept this quotation
        quotation.Status = "Accepted";
        quotation.UpdatedBy = "system";
        await _repository.UpdateAsync(quotation, cancellationToken).ConfigureAwait(false);

        // Get quotation items
        var quotationItems = await _quotationItemRepository.GetByFilterCriteriaAsync(
            new Common.QuotationItem.QuotationItemFilter { QuotationId = quotationId },
            cancellationToken).ConfigureAwait(false);

        // Create PO from quotation
        var po = new Common.PurchaseOrder.PurchaseOrder
        {
            VendorId = quotation.VendorId,
            OrderDate = DateTimeOffset.UtcNow,
            Status = "Pending",
            QuotationId = quotationId,
            TotalAmount = quotation.TotalAmount,
            BranchId = branchId,
            Notes = $"Auto-created from quotation {quotationId}",
            UpdatedBy = "system"
        };

        var createdPo = await _purchaseOrderRepository.AddAsync(po, cancellationToken).ConfigureAwait(false);

        if (createdPo is not null && quotationItems is not null)
        {
            foreach (var qItem in quotationItems)
            {
                await _purchaseOrderItemRepository.AddAsync(new Common.PurchaseOrderItem.PurchaseOrderItem
                {
                    PurchaseOrderId = createdPo.Id,
                    DrugId = qItem.DrugId,
                    QuantityOrdered = qItem.QuantityOffered,
                    QuantityReceived = 0,
                    UnitPrice = qItem.UnitPrice,
                    UpdatedBy = "system"
                }, cancellationToken).ConfigureAwait(false);
            }
        }

        _logger.LogDebug("Accepted quotation {QuotationId}. Created PO {PoId}.", quotationId, createdPo?.Id);

        return createdPo;
    }
}
