using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Drug;

namespace PharmacyManagementSystem.Server.Drug;

public class DrugRepository(ILogger<DrugRepository> logger, IDrugStorageClient storageClient) : IDrugRepository
{
    private readonly ILogger<DrugRepository> _logger = logger;
    private readonly IDrugStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Drug.Drug>?> GetByFilterCriteriaAsync(DrugFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting drugs by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} drugs.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Drug.Drug?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting drug by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved drug with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Drug.Drug?> AddAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drug);

        _logger.LogDebug("Repository: Adding drug with name: {Name}.", drug.Name);

        var result = await _storageClient.AddAsync(drug, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added drug with name: {Name}.", drug.Name);

        return result;
    }

    public async Task<Common.Drug.Drug?> UpdateAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drug);

        _logger.LogDebug("Repository: Updating drug with id: {Id}.", drug.Id);

        var result = await _storageClient.UpdateAsync(drug, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated drug with id: {Id}.", drug.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing drug with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed drug with id: {Id}.", id);
    }
}
