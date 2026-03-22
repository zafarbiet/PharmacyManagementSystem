using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventory;

namespace PharmacyManagementSystem.Server.DrugInventory;

public class GetDrugInventoryAction(ILogger<GetDrugInventoryAction> logger, IDrugInventoryRepository repository) : IGetDrugInventoryAction
{
    private readonly ILogger<GetDrugInventoryAction> _logger = logger;
    private readonly IDrugInventoryRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DrugInventory.DrugInventory>?> GetByFilterCriteriaAsync(DrugInventoryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting drug inventories by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} drug inventories.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugInventory.DrugInventory?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting drug inventory by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved drug inventory with id: {Id}.", id);

        return result;
    }
}
