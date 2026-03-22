using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.QuotationRequestItem;

public class SaveQuotationRequestItemAction(ILogger<SaveQuotationRequestItemAction> logger, IQuotationRequestItemRepository repository) : ISaveQuotationRequestItemAction
{
    private readonly ILogger<SaveQuotationRequestItemAction> _logger = logger;
    private readonly IQuotationRequestItemRepository _repository = repository;

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> AddAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);

        if (quotationRequestItem.QuotationRequestId == Guid.Empty)
            throw new BadRequestException("QuotationRequestItem QuotationRequestId is required.");

        if (quotationRequestItem.DrugId == Guid.Empty)
            throw new BadRequestException("QuotationRequestItem DrugId is required.");

        if (quotationRequestItem.QuantityRequired <= 0)
            throw new BadRequestException("QuotationRequestItem QuantityRequired must be greater than zero.");

        quotationRequestItem.UpdatedBy = "system";

        _logger.LogDebug("Adding new quotation request item.");

        var result = await _repository.AddAsync(quotationRequestItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added quotation request item.");

        return result;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> UpdateAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);

        if (quotationRequestItem.QuotationRequestId == Guid.Empty)
            throw new BadRequestException("QuotationRequestItem QuotationRequestId is required.");

        if (quotationRequestItem.DrugId == Guid.Empty)
            throw new BadRequestException("QuotationRequestItem DrugId is required.");

        if (quotationRequestItem.QuantityRequired <= 0)
            throw new BadRequestException("QuotationRequestItem QuantityRequired must be greater than zero.");

        quotationRequestItem.UpdatedBy = "system";

        _logger.LogDebug("Updating quotation request item with id: {Id}.", quotationRequestItem.Id);

        var result = await _repository.UpdateAsync(quotationRequestItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated quotation request item with id: {Id}.", quotationRequestItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing quotation request item with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed quotation request item with id: {Id}.", id);
    }
}
