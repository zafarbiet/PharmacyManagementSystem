using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Drug;

namespace PharmacyManagementSystem.Server.Drug;

public class GetDrugAction(ILogger<GetDrugAction> logger, IDrugRepository repository) : IGetDrugAction
{
    private readonly ILogger<GetDrugAction> _logger = logger;
    private readonly IDrugRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Drug.Drug>?> GetByFilterCriteriaAsync(DrugFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting drugs by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} drugs.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Drug.Drug?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting drug by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved drug with id: {Id}.", id);

        return result;
    }
}
