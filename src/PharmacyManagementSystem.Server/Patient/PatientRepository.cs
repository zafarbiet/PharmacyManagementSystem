using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Patient;

namespace PharmacyManagementSystem.Server.Patient;

public class PatientRepository(ILogger<PatientRepository> logger, IPatientStorageClient storageClient) : IPatientRepository
{
    private readonly ILogger<PatientRepository> _logger = logger;
    private readonly IPatientStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Patient.Patient>?> GetByFilterCriteriaAsync(PatientFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting patients by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} patients.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Patient.Patient?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting patient by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved patient with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Patient.Patient?> AddAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(patient);

        _logger.LogDebug("Repository: Adding patient with name: {Name}.", patient.Name);

        var result = await _storageClient.AddAsync(patient, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added patient with name: {Name}.", patient.Name);

        return result;
    }

    public async Task<Common.Patient.Patient?> UpdateAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(patient);

        _logger.LogDebug("Repository: Updating patient with id: {Id}.", patient.Id);

        var result = await _storageClient.UpdateAsync(patient, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated patient with id: {Id}.", patient.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing patient with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed patient with id: {Id}.", id);
    }
}
