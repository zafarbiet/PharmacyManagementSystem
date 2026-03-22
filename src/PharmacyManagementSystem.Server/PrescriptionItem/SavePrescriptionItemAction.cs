using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.PrescriptionItem;

public class SavePrescriptionItemAction(ILogger<SavePrescriptionItemAction> logger, IPrescriptionItemRepository repository) : ISavePrescriptionItemAction
{
    private readonly ILogger<SavePrescriptionItemAction> _logger = logger;
    private readonly IPrescriptionItemRepository _repository = repository;

    public async Task<Common.PrescriptionItem.PrescriptionItem?> AddAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);

        if (prescriptionItem.PrescriptionId == Guid.Empty)
            throw new BadRequestException("PrescriptionItem PrescriptionId is required.");

        if (prescriptionItem.DrugId == Guid.Empty)
            throw new BadRequestException("PrescriptionItem DrugId is required.");

        prescriptionItem.UpdatedBy = "system";

        _logger.LogDebug("Adding new prescription item for PrescriptionId: {PrescriptionId}.", prescriptionItem.PrescriptionId);

        var result = await _repository.AddAsync(prescriptionItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added prescription item for PrescriptionId: {PrescriptionId}.", prescriptionItem.PrescriptionId);

        return result;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> UpdateAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);

        if (prescriptionItem.PrescriptionId == Guid.Empty)
            throw new BadRequestException("PrescriptionItem PrescriptionId is required.");

        if (prescriptionItem.DrugId == Guid.Empty)
            throw new BadRequestException("PrescriptionItem DrugId is required.");

        prescriptionItem.UpdatedBy = "system";

        _logger.LogDebug("Updating prescription item with id: {Id}.", prescriptionItem.Id);

        var result = await _repository.UpdateAsync(prescriptionItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated prescription item with id: {Id}.", prescriptionItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing prescription item with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed prescription item with id: {Id}.", id);
    }
}
