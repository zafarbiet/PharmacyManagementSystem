using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.CustomerInvoice;

public class SaveCustomerInvoiceAction(ILogger<SaveCustomerInvoiceAction> logger, ICustomerInvoiceRepository repository) : ISaveCustomerInvoiceAction
{
    private readonly ILogger<SaveCustomerInvoiceAction> _logger = logger;
    private readonly ICustomerInvoiceRepository _repository = repository;

    public async Task<Common.CustomerInvoice.CustomerInvoice?> AddAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        if (string.IsNullOrWhiteSpace(customerInvoice.Status))
            throw new BadRequestException("CustomerInvoice Status is required.");

        customerInvoice.UpdatedBy = "system";

        _logger.LogDebug("Adding new customer invoice with status: {Status}.", customerInvoice.Status);

        var result = await _repository.AddAsync(customerInvoice, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added customer invoice with status: {Status}.", customerInvoice.Status);

        return result;
    }

    public async Task<Common.CustomerInvoice.CustomerInvoice?> UpdateAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        if (string.IsNullOrWhiteSpace(customerInvoice.Status))
            throw new BadRequestException("CustomerInvoice Status is required.");

        customerInvoice.UpdatedBy = "system";

        _logger.LogDebug("Updating customer invoice with id: {Id}.", customerInvoice.Id);

        var result = await _repository.UpdateAsync(customerInvoice, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated customer invoice with id: {Id}.", customerInvoice.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing customer invoice with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed customer invoice with id: {Id}.", id);
    }
}
