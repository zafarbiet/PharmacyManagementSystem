using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationRequestItem;

namespace PharmacyManagementSystem.Server.QuotationRequestItem;

public class QuotationRequestItemRepository(ILogger<QuotationRequestItemRepository> logger, IQuotationRequestItemStorageClient storageClient) : IQuotationRequestItemRepository
{
    private readonly ILogger<QuotationRequestItemRepository> _logger = logger;
    private readonly IQuotationRequestItemStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.QuotationRequestItem.QuotationRequestItem>?> GetByFilterCriteriaAsync(QuotationRequestItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting quotation request items by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} quotation request items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting quotation request item by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved quotation request item with id: {Id}.", id);

        return result;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> AddAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);

        _logger.LogDebug("Repository: Adding quotation request item.");

        var result = await _storageClient.AddAsync(quotationRequestItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added quotation request item.");

        return result;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> UpdateAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequestItem);

        _logger.LogDebug("Repository: Updating quotation request item with id: {Id}.", quotationRequestItem.Id);

        var result = await _storageClient.UpdateAsync(quotationRequestItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated quotation request item with id: {Id}.", quotationRequestItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing quotation request item with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed quotation request item with id: {Id}.", id);
    }
}
