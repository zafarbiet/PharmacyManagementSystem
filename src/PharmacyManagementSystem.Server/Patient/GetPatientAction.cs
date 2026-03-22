using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Patient;

namespace PharmacyManagementSystem.Server.Patient;

public class GetPatientAction(ILogger<GetPatientAction> logger, IPatientRepository repository) : IGetPatientAction
{
    private readonly ILogger<GetPatientAction> _logger = logger;
    private readonly IPatientRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Patient.Patient>?> GetByFilterCriteriaAsync(PatientFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting patients by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} patients.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Patient.Patient?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting patient by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved patient with id: {Id}.", id);

        return result;
    }
}
