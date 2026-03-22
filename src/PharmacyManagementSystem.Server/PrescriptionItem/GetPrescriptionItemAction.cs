using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PrescriptionItem;

namespace PharmacyManagementSystem.Server.PrescriptionItem;

public class GetPrescriptionItemAction(ILogger<GetPrescriptionItemAction> logger, IPrescriptionItemRepository repository) : IGetPrescriptionItemAction
{
    private readonly ILogger<GetPrescriptionItemAction> _logger = logger;
    private readonly IPrescriptionItemRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.PrescriptionItem.PrescriptionItem>?> GetByFilterCriteriaAsync(PrescriptionItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting prescription items by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} prescription items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting prescription item by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved prescription item with id: {Id}.", id);

        return result;
    }
}
