using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Prescription;

namespace PharmacyManagementSystem.Server.Prescription;

public class GetPrescriptionAction(ILogger<GetPrescriptionAction> logger, IPrescriptionRepository repository) : IGetPrescriptionAction
{
    private readonly ILogger<GetPrescriptionAction> _logger = logger;
    private readonly IPrescriptionRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Prescription.Prescription>?> GetByFilterCriteriaAsync(PrescriptionFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting prescriptions by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} prescriptions.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Prescription.Prescription?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting prescription by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved prescription with id: {Id}.", id);

        return result;
    }
}
