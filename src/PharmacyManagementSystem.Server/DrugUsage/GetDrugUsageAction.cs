using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugUsage;

namespace PharmacyManagementSystem.Server.DrugUsage;

public class GetDrugUsageAction(ILogger<GetDrugUsageAction> logger, IDrugUsageRepository repository) : IGetDrugUsageAction
{
    private readonly ILogger<GetDrugUsageAction> _logger = logger;
    private readonly IDrugUsageRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DrugUsage.DrugUsage>?> GetByFilterCriteriaAsync(DrugUsageFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting drug usages by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} drug usages.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugUsage.DrugUsage?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting drug usage by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved drug usage with id: {Id}.", id);

        return result;
    }
}
