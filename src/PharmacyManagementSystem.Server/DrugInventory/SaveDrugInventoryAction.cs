using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DrugInventory;

public class SaveDrugInventoryAction(ILogger<SaveDrugInventoryAction> logger, IDrugInventoryRepository repository) : ISaveDrugInventoryAction
{
    private readonly ILogger<SaveDrugInventoryAction> _logger = logger;
    private readonly IDrugInventoryRepository _repository = repository;

    public async Task<Common.DrugInventory.DrugInventory?> AddAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        if (drugInventory.DrugId == Guid.Empty)
            throw new BadRequestException("DrugInventory DrugId is required.");

        if (string.IsNullOrWhiteSpace(drugInventory.BatchNumber))
            throw new BadRequestException("DrugInventory BatchNumber is required.");

        if (drugInventory.QuantityInStock < 0)
            throw new BadRequestException("DrugInventory QuantityInStock must be >= 0.");

        drugInventory.UpdatedBy = "system";

        _logger.LogDebug("Adding new drug inventory for DrugId: {DrugId}.", drugInventory.DrugId);

        var result = await _repository.AddAsync(drugInventory, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added drug inventory for DrugId: {DrugId}.", drugInventory.DrugId);

        return result;
    }

    public async Task<Common.DrugInventory.DrugInventory?> UpdateAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        if (drugInventory.DrugId == Guid.Empty)
            throw new BadRequestException("DrugInventory DrugId is required.");

        if (string.IsNullOrWhiteSpace(drugInventory.BatchNumber))
            throw new BadRequestException("DrugInventory BatchNumber is required.");

        if (drugInventory.QuantityInStock < 0)
            throw new BadRequestException("DrugInventory QuantityInStock must be >= 0.");

        drugInventory.UpdatedBy = "system";

        _logger.LogDebug("Updating drug inventory with id: {Id}.", drugInventory.Id);

        var result = await _repository.UpdateAsync(drugInventory, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated drug inventory with id: {Id}.", drugInventory.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing drug inventory with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed drug inventory with id: {Id}.", id);
    }
}
