using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugCategory;

namespace PharmacyManagementSystem.Server.DrugCategory;

public class DrugCategoryRepository(ILogger<DrugCategoryRepository> logger, IDrugCategoryStorageClient storageClient) : IDrugCategoryRepository
{
    private readonly ILogger<DrugCategoryRepository> _logger = logger;
    private readonly IDrugCategoryStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DrugCategory.DrugCategory>?> GetByFilterCriteriaAsync(DrugCategoryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting drug categories by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} drug categories.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugCategory.DrugCategory?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting drug category by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved drug category with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DrugCategory.DrugCategory?> AddAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(category);

        _logger.LogDebug("Repository: Adding drug category with name: {Name}.", category.Name);

        var result = await _storageClient.AddAsync(category, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added drug category with name: {Name}.", category.Name);

        return result;
    }

    public async Task<Common.DrugCategory.DrugCategory?> UpdateAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(category);

        _logger.LogDebug("Repository: Updating drug category with id: {Id}.", category.Id);

        var result = await _storageClient.UpdateAsync(category, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated drug category with id: {Id}.", category.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing drug category with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed drug category with id: {Id}.", id);
    }
}
