using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerInvoiceItem;

namespace PharmacyManagementSystem.Server.CustomerInvoiceItem;

public class CustomerInvoiceItemRepository(ILogger<CustomerInvoiceItemRepository> logger, ICustomerInvoiceItemStorageClient storageClient) : ICustomerInvoiceItemRepository
{
    private readonly ILogger<CustomerInvoiceItemRepository> _logger = logger;
    private readonly ICustomerInvoiceItemStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.CustomerInvoiceItem.CustomerInvoiceItem>?> GetByFilterCriteriaAsync(CustomerInvoiceItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting customer invoice items by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} customer invoice items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting customer invoice item by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved customer invoice item with id: {Id}.", id);

        return result;
    }

    public async Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> AddAsync(Common.CustomerInvoiceItem.CustomerInvoiceItem? customerInvoiceItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoiceItem);

        _logger.LogDebug("Repository: Adding customer invoice item for InvoiceId: {InvoiceId}.", customerInvoiceItem.InvoiceId);

        var result = await _storageClient.AddAsync(customerInvoiceItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added customer invoice item for InvoiceId: {InvoiceId}.", customerInvoiceItem.InvoiceId);

        return result;
    }

    public async Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> UpdateAsync(Common.CustomerInvoiceItem.CustomerInvoiceItem? customerInvoiceItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoiceItem);

        _logger.LogDebug("Repository: Updating customer invoice item with id: {Id}.", customerInvoiceItem.Id);

        var result = await _storageClient.UpdateAsync(customerInvoiceItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated customer invoice item with id: {Id}.", customerInvoiceItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing customer invoice item with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed customer invoice item with id: {Id}.", id);
    }
}
