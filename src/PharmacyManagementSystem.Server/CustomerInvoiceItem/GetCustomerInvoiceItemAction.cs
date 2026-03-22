using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerInvoiceItem;

namespace PharmacyManagementSystem.Server.CustomerInvoiceItem;

public class GetCustomerInvoiceItemAction(ILogger<GetCustomerInvoiceItemAction> logger, ICustomerInvoiceItemRepository repository) : IGetCustomerInvoiceItemAction
{
    private readonly ILogger<GetCustomerInvoiceItemAction> _logger = logger;
    private readonly ICustomerInvoiceItemRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.CustomerInvoiceItem.CustomerInvoiceItem>?> GetByFilterCriteriaAsync(CustomerInvoiceItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting customer invoice items by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} customer invoice items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting customer invoice item by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved customer invoice item with id: {Id}.", id);

        return result;
    }
}
