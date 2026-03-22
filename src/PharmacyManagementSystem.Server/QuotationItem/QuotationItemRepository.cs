using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationItem;

namespace PharmacyManagementSystem.Server.QuotationItem;

public class QuotationItemRepository(ILogger<QuotationItemRepository> logger, IQuotationItemStorageClient storageClient) : IQuotationItemRepository
{
    private readonly ILogger<QuotationItemRepository> _logger = logger;
    private readonly IQuotationItemStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.QuotationItem.QuotationItem>?> GetByFilterCriteriaAsync(QuotationItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting quotation items by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} quotation items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.QuotationItem.QuotationItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting quotation item by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved quotation item with id: {Id}.", id);

        return result;
    }

    public async Task<Common.QuotationItem.QuotationItem?> AddAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);

        _logger.LogDebug("Repository: Adding quotation item.");

        var result = await _storageClient.AddAsync(quotationItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added quotation item.");

        return result;
    }

    public async Task<Common.QuotationItem.QuotationItem?> UpdateAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationItem);

        _logger.LogDebug("Repository: Updating quotation item with id: {Id}.", quotationItem.Id);

        var result = await _storageClient.UpdateAsync(quotationItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated quotation item with id: {Id}.", quotationItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing quotation item with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed quotation item with id: {Id}.", id);
    }
}
