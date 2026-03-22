using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.QuotationItem;

public class SaveQuotationItemAction(ILogger<SaveQuotationItemAction> logger, IQuotationItemRepository repository) : ISaveQuotationItemAction
{
    private readonly ILogger<SaveQuotationItemAction> _logger = logger;
    private readonly IQuotationItemRepository _repository = repository;

    public async Task<Common.QuotationItem.QuotationItem?> AddAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);

        if (quotationItem.QuotationId == Guid.Empty)
            throw new BadRequestException("QuotationItem QuotationId is required.");

        if (quotationItem.DrugId == Guid.Empty)
            throw new BadRequestException("QuotationItem DrugId is required.");

        if (quotationItem.QuantityOffered <= 0)
            throw new BadRequestException("QuotationItem QuantityOffered must be greater than zero.");

        if (quotationItem.UnitPrice <= 0)
            throw new BadRequestException("QuotationItem UnitPrice must be greater than zero.");

        quotationItem.UpdatedBy = "system";

        _logger.LogDebug("Adding new quotation item.");

        var result = await _repository.AddAsync(quotationItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added quotation item.");

        return result;
    }

    public async Task<Common.QuotationItem.QuotationItem?> UpdateAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);

        if (quotationItem.QuotationId == Guid.Empty)
            throw new BadRequestException("QuotationItem QuotationId is required.");

        if (quotationItem.DrugId == Guid.Empty)
            throw new BadRequestException("QuotationItem DrugId is required.");

        if (quotationItem.QuantityOffered <= 0)
            throw new BadRequestException("QuotationItem QuantityOffered must be greater than zero.");

        if (quotationItem.UnitPrice <= 0)
            throw new BadRequestException("QuotationItem UnitPrice must be greater than zero.");

        quotationItem.UpdatedBy = "system";

        _logger.LogDebug("Updating quotation item with id: {Id}.", quotationItem.Id);

        var result = await _repository.UpdateAsync(quotationItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated quotation item with id: {Id}.", quotationItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing quotation item with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed quotation item with id: {Id}.", id);
    }
}
