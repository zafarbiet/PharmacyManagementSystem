using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationVendorResponse;

namespace PharmacyManagementSystem.Server.QuotationVendorResponse;

public class GetQuotationVendorResponseAction(ILogger<GetQuotationVendorResponseAction> logger, IQuotationVendorResponseRepository repository) : IGetQuotationVendorResponseAction
{
    private readonly ILogger<GetQuotationVendorResponseAction> _logger = logger;
    private readonly IQuotationVendorResponseRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.QuotationVendorResponse.QuotationVendorResponse>?> GetByFilterCriteriaAsync(QuotationVendorResponseFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting quotation vendor responses by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} quotation vendor responses.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.QuotationVendorResponse.QuotationVendorResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting quotation vendor response by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved quotation vendor response with id: {Id}.", id);

        return result;
    }
}
