using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Quotation;

namespace PharmacyManagementSystem.Server.Quotation;

public class QuotationRepository(ILogger<QuotationRepository> logger, IQuotationStorageClient storageClient) : IQuotationRepository
{
    private readonly ILogger<QuotationRepository> _logger = logger;
    private readonly IQuotationStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Quotation.Quotation>?> GetByFilterCriteriaAsync(QuotationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting quotations by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} quotations.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Quotation.Quotation?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting quotation by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved quotation with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Quotation.Quotation?> AddAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        _logger.LogDebug("Repository: Adding quotation.");

        var result = await _storageClient.AddAsync(quotation, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added quotation.");

        return result;
    }

    public async Task<Common.Quotation.Quotation?> UpdateAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        _logger.LogDebug("Repository: Updating quotation with id: {Id}.", quotation.Id);

        var result = await _storageClient.UpdateAsync(quotation, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated quotation with id: {Id}.", quotation.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing quotation with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed quotation with id: {Id}.", id);
    }
}
