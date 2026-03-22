using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationRequest;

namespace PharmacyManagementSystem.Server.QuotationRequest;

public class GetQuotationRequestAction(ILogger<GetQuotationRequestAction> logger, IQuotationRequestRepository repository) : IGetQuotationRequestAction
{
    private readonly ILogger<GetQuotationRequestAction> _logger = logger;
    private readonly IQuotationRequestRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.QuotationRequest.QuotationRequest>?> GetByFilterCriteriaAsync(QuotationRequestFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting quotation requests by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} quotation requests.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting quotation request by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved quotation request with id: {Id}.", id);

        return result;
    }
}
