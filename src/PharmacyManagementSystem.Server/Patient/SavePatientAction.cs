using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Patient;

public class SavePatientAction(ILogger<SavePatientAction> logger, IPatientRepository repository) : ISavePatientAction
{
    private readonly ILogger<SavePatientAction> _logger = logger;
    private readonly IPatientRepository _repository = repository;

    public async Task<Common.Patient.Patient?> AddAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(patient);

        if (string.IsNullOrWhiteSpace(patient.Name))
            throw new BadRequestException("Patient Name is required.");

        patient.UpdatedBy = "system";

        _logger.LogDebug("Adding new patient with name: {Name}.", patient.Name);

        var result = await _repository.AddAsync(patient, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added patient with name: {Name}.", patient.Name);

        return result;
    }

    public async Task<Common.Patient.Patient?> UpdateAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(patient);

        if (string.IsNullOrWhiteSpace(patient.Name))
            throw new BadRequestException("Patient Name is required.");

        patient.UpdatedBy = "system";

        _logger.LogDebug("Updating patient with id: {Id}.", patient.Id);

        var result = await _repository.UpdateAsync(patient, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated patient with id: {Id}.", patient.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing patient with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed patient with id: {Id}.", id);
    }
}
