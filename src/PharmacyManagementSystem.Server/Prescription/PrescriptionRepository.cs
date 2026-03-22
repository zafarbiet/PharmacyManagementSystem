using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Prescription;

namespace PharmacyManagementSystem.Server.Prescription;

public class PrescriptionRepository(ILogger<PrescriptionRepository> logger, IPrescriptionStorageClient storageClient) : IPrescriptionRepository
{
    private readonly ILogger<PrescriptionRepository> _logger = logger;
    private readonly IPrescriptionStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Prescription.Prescription>?> GetByFilterCriteriaAsync(PrescriptionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting prescriptions by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} prescriptions.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Prescription.Prescription?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting prescription by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved prescription with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Prescription.Prescription?> AddAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        _logger.LogDebug("Repository: Adding prescription for PatientId: {PatientId}.", prescription.PatientId);

        var result = await _storageClient.AddAsync(prescription, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added prescription for PatientId: {PatientId}.", prescription.PatientId);

        return result;
    }

    public async Task<Common.Prescription.Prescription?> UpdateAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescription);

        _logger.LogDebug("Repository: Updating prescription with id: {Id}.", prescription.Id);

        var result = await _storageClient.UpdateAsync(prescription, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated prescription with id: {Id}.", prescription.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing prescription with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed prescription with id: {Id}.", id);
    }
}
