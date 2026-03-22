using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Quotation;

namespace PharmacyManagementSystem.Server.Quotation;

public class GetQuotationAction(ILogger<GetQuotationAction> logger, IQuotationRepository repository) : IGetQuotationAction
{
    private readonly ILogger<GetQuotationAction> _logger = logger;
    private readonly IQuotationRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Quotation.Quotation>?> GetByFilterCriteriaAsync(QuotationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting quotations by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} quotations.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Quotation.Quotation?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting quotation by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved quotation with id: {Id}.", id);

        return result;
    }
}
