using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PurchaseOrderItem;

namespace PharmacyManagementSystem.Server.PurchaseOrderItem;

public class GetPurchaseOrderItemAction(ILogger<GetPurchaseOrderItemAction> logger, IPurchaseOrderItemRepository repository) : IGetPurchaseOrderItemAction
{
    private readonly ILogger<GetPurchaseOrderItemAction> _logger = logger;
    private readonly IPurchaseOrderItemRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.PurchaseOrderItem.PurchaseOrderItem>?> GetByFilterCriteriaAsync(PurchaseOrderItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting purchase order items by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} purchase order items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.PurchaseOrderItem.PurchaseOrderItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting purchase order item by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved purchase order item with id: {Id}.", id);

        return result;
    }
}
