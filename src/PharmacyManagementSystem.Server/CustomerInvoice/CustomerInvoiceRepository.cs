using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerInvoice;

namespace PharmacyManagementSystem.Server.CustomerInvoice;

public class CustomerInvoiceRepository(ILogger<CustomerInvoiceRepository> logger, ICustomerInvoiceStorageClient storageClient) : ICustomerInvoiceRepository
{
    private readonly ILogger<CustomerInvoiceRepository> _logger = logger;
    private readonly ICustomerInvoiceStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.CustomerInvoice.CustomerInvoice>?> GetByFilterCriteriaAsync(CustomerInvoiceFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting customer invoices by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} customer invoices.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.CustomerInvoice.CustomerInvoice?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting customer invoice by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved customer invoice with id: {Id}.", id);

        return result;
    }

    public async Task<Common.CustomerInvoice.CustomerInvoice?> AddAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        _logger.LogDebug("Repository: Adding customer invoice with status: {Status}.", customerInvoice.Status);

        var result = await _storageClient.AddAsync(customerInvoice, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added customer invoice with status: {Status}.", customerInvoice.Status);

        return result;
    }

    public async Task<Common.CustomerInvoice.CustomerInvoice?> UpdateAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        _logger.LogDebug("Repository: Updating customer invoice with id: {Id}.", customerInvoice.Id);

        var result = await _storageClient.UpdateAsync(customerInvoice, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated customer invoice with id: {Id}.", customerInvoice.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing customer invoice with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed customer invoice with id: {Id}.", id);
    }

    public async Task<string> GetNextInvoiceNumberAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Repository: Getting next invoice number.");

        var result = await _storageClient.GetNextInvoiceNumberAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Next invoice number is {InvoiceNumber}.", result);

        return result;
    }
}
