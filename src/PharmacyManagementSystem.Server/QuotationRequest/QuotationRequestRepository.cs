using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationRequest;

namespace PharmacyManagementSystem.Server.QuotationRequest;

public class QuotationRequestRepository(ILogger<QuotationRequestRepository> logger, IQuotationRequestStorageClient storageClient) : IQuotationRequestRepository
{
    private readonly ILogger<QuotationRequestRepository> _logger = logger;
    private readonly IQuotationRequestStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.QuotationRequest.QuotationRequest>?> GetByFilterCriteriaAsync(QuotationRequestFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting quotation requests by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} quotation requests.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting quotation request by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved quotation request with id: {Id}.", id);

        return result;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> AddAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);

        _logger.LogDebug("Repository: Adding quotation request.");

        var result = await _storageClient.AddAsync(quotationRequest, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added quotation request.");

        return result;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> UpdateAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);

        _logger.LogDebug("Repository: Updating quotation request with id: {Id}.", quotationRequest.Id);

        var result = await _storageClient.UpdateAsync(quotationRequest, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated quotation request with id: {Id}.", quotationRequest.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing quotation request with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed quotation request with id: {Id}.", id);
    }
}
