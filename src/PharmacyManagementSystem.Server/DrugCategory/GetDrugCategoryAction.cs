using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugCategory;

namespace PharmacyManagementSystem.Server.DrugCategory;

public class GetDrugCategoryAction(ILogger<GetDrugCategoryAction> logger, IDrugCategoryRepository repository) : IGetDrugCategoryAction
{
    private readonly ILogger<GetDrugCategoryAction> _logger = logger;
    private readonly IDrugCategoryRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DrugCategory.DrugCategory>?> GetByFilterCriteriaAsync(DrugCategoryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting drug categories by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} drug categories.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugCategory.DrugCategory?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting drug category by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved drug category with id: {Id}.", id);

        return result;
    }
}
