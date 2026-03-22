using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerInvoice;

namespace PharmacyManagementSystem.Server.CustomerInvoice;

public class GetCustomerInvoiceAction(ILogger<GetCustomerInvoiceAction> logger, ICustomerInvoiceRepository repository) : IGetCustomerInvoiceAction
{
    private readonly ILogger<GetCustomerInvoiceAction> _logger = logger;
    private readonly ICustomerInvoiceRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.CustomerInvoice.CustomerInvoice>?> GetByFilterCriteriaAsync(CustomerInvoiceFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting customer invoices by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} customer invoices.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.CustomerInvoice.CustomerInvoice?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting customer invoice by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved customer invoice with id: {Id}.", id);

        return result;
    }
}
