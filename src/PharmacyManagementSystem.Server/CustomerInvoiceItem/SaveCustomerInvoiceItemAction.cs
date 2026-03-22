using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.CustomerInvoiceItem;

public class SaveCustomerInvoiceItemAction(ILogger<SaveCustomerInvoiceItemAction> logger, ICustomerInvoiceItemRepository repository) : ISaveCustomerInvoiceItemAction
{
    private readonly ILogger<SaveCustomerInvoiceItemAction> _logger = logger;
    private readonly ICustomerInvoiceItemRepository _repository = repository;

    public async Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> AddAsync(Common.CustomerInvoiceItem.CustomerInvoiceItem? customerInvoiceItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoiceItem);

        if (customerInvoiceItem.InvoiceId == Guid.Empty)
            throw new BadRequestException("CustomerInvoiceItem InvoiceId is required.");

        if (customerInvoiceItem.DrugId == Guid.Empty)
            throw new BadRequestException("CustomerInvoiceItem DrugId is required.");

        if (customerInvoiceItem.Quantity <= 0)
            throw new BadRequestException("CustomerInvoiceItem Quantity must be > 0.");

        customerInvoiceItem.UpdatedBy = "system";

        _logger.LogDebug("Adding new customer invoice item for InvoiceId: {InvoiceId}.", customerInvoiceItem.InvoiceId);

        var result = await _repository.AddAsync(customerInvoiceItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added customer invoice item for InvoiceId: {InvoiceId}.", customerInvoiceItem.InvoiceId);

        return result;
    }

    public async Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> UpdateAsync(Common.CustomerInvoiceItem.CustomerInvoiceItem? customerInvoiceItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoiceItem);

        if (customerInvoiceItem.InvoiceId == Guid.Empty)
            throw new BadRequestException("CustomerInvoiceItem InvoiceId is required.");

        if (customerInvoiceItem.DrugId == Guid.Empty)
            throw new BadRequestException("CustomerInvoiceItem DrugId is required.");

        if (customerInvoiceItem.Quantity <= 0)
            throw new BadRequestException("CustomerInvoiceItem Quantity must be > 0.");

        customerInvoiceItem.UpdatedBy = "system";

        _logger.LogDebug("Updating customer invoice item with id: {Id}.", customerInvoiceItem.Id);

        var result = await _repository.UpdateAsync(customerInvoiceItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated customer invoice item with id: {Id}.", customerInvoiceItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing customer invoice item with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed customer invoice item with id: {Id}.", id);
    }
}
