using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationVendorResponse;

namespace PharmacyManagementSystem.Server.QuotationVendorResponse;

public class QuotationVendorResponseRepository(ILogger<QuotationVendorResponseRepository> logger, IQuotationVendorResponseStorageClient storageClient) : IQuotationVendorResponseRepository
{
    private readonly ILogger<QuotationVendorResponseRepository> _logger = logger;
    private readonly IQuotationVendorResponseStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.QuotationVendorResponse.QuotationVendorResponse>?> GetByFilterCriteriaAsync(QuotationVendorResponseFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting quotation vendor responses by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} quotation vendor responses.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.QuotationVendorResponse.QuotationVendorResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting quotation vendor response by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved quotation vendor response with id: {Id}.", id);

        return result;
    }

    public async Task<Common.QuotationVendorResponse.QuotationVendorResponse?> AddAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response);

        _logger.LogDebug("Repository: Adding quotation vendor response.");

        var result = await _storageClient.AddAsync(response, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added quotation vendor response with id: {Id}.", result?.Id);

        return result;
    }

    public async Task<Common.QuotationVendorResponse.QuotationVendorResponse?> UpdateAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response);

        _logger.LogDebug("Repository: Updating quotation vendor response with id: {Id}.", response.Id);

        var result = await _storageClient.UpdateAsync(response, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated quotation vendor response with id: {Id}.", response.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing quotation vendor response with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed quotation vendor response with id: {Id}.", id);
    }
}
