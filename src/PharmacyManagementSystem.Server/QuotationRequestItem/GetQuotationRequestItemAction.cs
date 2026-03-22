using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationRequestItem;

namespace PharmacyManagementSystem.Server.QuotationRequestItem;

public class GetQuotationRequestItemAction(ILogger<GetQuotationRequestItemAction> logger, IQuotationRequestItemRepository repository) : IGetQuotationRequestItemAction
{
    private readonly ILogger<GetQuotationRequestItemAction> _logger = logger;
    private readonly IQuotationRequestItemRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.QuotationRequestItem.QuotationRequestItem>?> GetByFilterCriteriaAsync(QuotationRequestItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting quotation request items by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} quotation request items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.QuotationRequestItem.QuotationRequestItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting quotation request item by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved quotation request item with id: {Id}.", id);

        return result;
    }
}
