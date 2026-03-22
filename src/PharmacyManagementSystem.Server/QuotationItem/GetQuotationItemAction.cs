using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationItem;

namespace PharmacyManagementSystem.Server.QuotationItem;

public class GetQuotationItemAction(ILogger<GetQuotationItemAction> logger, IQuotationItemRepository repository) : IGetQuotationItemAction
{
    private readonly ILogger<GetQuotationItemAction> _logger = logger;
    private readonly IQuotationItemRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.QuotationItem.QuotationItem>?> GetByFilterCriteriaAsync(QuotationItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting quotation items by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} quotation items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.QuotationItem.QuotationItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting quotation item by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved quotation item with id: {Id}.", id);

        return result;
    }
}
