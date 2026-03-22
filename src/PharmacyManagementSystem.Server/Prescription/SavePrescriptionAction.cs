using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Prescription;

public class SavePrescriptionAction(ILogger<SavePrescriptionAction> logger, IPrescriptionRepository repository) : ISavePrescriptionAction
{
    private readonly ILogger<SavePrescriptionAction> _logger = logger;
    private readonly IPrescriptionRepository _repository = repository;

    public async Task<Common.Prescription.Prescription?> AddAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        if (prescription.PatientId == Guid.Empty)
            throw new BadRequestException("Prescription PatientId is required.");

        prescription.UpdatedBy = "system";

        _logger.LogDebug("Adding new prescription for PatientId: {PatientId}.", prescription.PatientId);

        var result = await _repository.AddAsync(prescription, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added prescription for PatientId: {PatientId}.", prescription.PatientId);

        return result;
    }

    public async Task<Common.Prescription.Prescription?> UpdateAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        if (prescription.PatientId == Guid.Empty)
            throw new BadRequestException("Prescription PatientId is required.");

        prescription.UpdatedBy = "system";

        _logger.LogDebug("Updating prescription with id: {Id}.", prescription.Id);

        var result = await _repository.UpdateAsync(prescription, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated prescription with id: {Id}.", prescription.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing prescription with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed prescription with id: {Id}.", id);
    }
}
