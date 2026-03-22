using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PurchaseOrder;

namespace PharmacyManagementSystem.Server.PurchaseOrder;

public class GetPurchaseOrderAction(ILogger<GetPurchaseOrderAction> logger, IPurchaseOrderRepository repository) : IGetPurchaseOrderAction
{
    private readonly ILogger<GetPurchaseOrderAction> _logger = logger;
    private readonly IPurchaseOrderRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.PurchaseOrder.PurchaseOrder>?> GetByFilterCriteriaAsync(PurchaseOrderFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting purchase orders by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} purchase orders.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.PurchaseOrder.PurchaseOrder?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting purchase order by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved purchase order with id: {Id}.", id);

        return result;
    }
}
