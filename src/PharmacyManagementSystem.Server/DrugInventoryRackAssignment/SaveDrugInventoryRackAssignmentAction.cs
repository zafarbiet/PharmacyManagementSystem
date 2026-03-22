using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DrugInventoryRackAssignment;

public class SaveDrugInventoryRackAssignmentAction(ILogger<SaveDrugInventoryRackAssignmentAction> logger, IDrugInventoryRackAssignmentRepository repository) : ISaveDrugInventoryRackAssignmentAction
{
    private readonly ILogger<SaveDrugInventoryRackAssignmentAction> _logger = logger;
    private readonly IDrugInventoryRackAssignmentRepository _repository = repository;

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> AddAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);

        if (drugInventoryRackAssignment.DrugInventoryId == Guid.Empty)
            throw new BadRequestException("DrugInventoryRackAssignment DrugInventoryId is required.");

        if (drugInventoryRackAssignment.RackId == Guid.Empty)
            throw new BadRequestException("DrugInventoryRackAssignment RackId is required.");

        if (drugInventoryRackAssignment.QuantityPlaced <= 0)
            throw new BadRequestException("DrugInventoryRackAssignment QuantityPlaced must be greater than zero.");

        drugInventoryRackAssignment.UpdatedBy = "system";

        _logger.LogDebug("Adding new drug inventory rack assignment for drug inventory: {DrugInventoryId}.", drugInventoryRackAssignment.DrugInventoryId);

        var result = await _repository.AddAsync(drugInventoryRackAssignment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added drug inventory rack assignment for drug inventory: {DrugInventoryId}.", drugInventoryRackAssignment.DrugInventoryId);

        return result;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> UpdateAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);

        if (drugInventoryRackAssignment.DrugInventoryId == Guid.Empty)
            throw new BadRequestException("DrugInventoryRackAssignment DrugInventoryId is required.");

        if (drugInventoryRackAssignment.RackId == Guid.Empty)
            throw new BadRequestException("DrugInventoryRackAssignment RackId is required.");

        if (drugInventoryRackAssignment.QuantityPlaced <= 0)
            throw new BadRequestException("DrugInventoryRackAssignment QuantityPlaced must be greater than zero.");

        drugInventoryRackAssignment.UpdatedBy = "system";

        _logger.LogDebug("Updating drug inventory rack assignment with id: {Id}.", drugInventoryRackAssignment.Id);

        var result = await _repository.UpdateAsync(drugInventoryRackAssignment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated drug inventory rack assignment with id: {Id}.", drugInventoryRackAssignment.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing drug inventory rack assignment with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed drug inventory rack assignment with id: {Id}.", id);
    }
}
