using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugUsage;

namespace PharmacyManagementSystem.Server.DrugUsage;

public class DrugUsageRepository(ILogger<DrugUsageRepository> logger, IDrugUsageStorageClient storageClient) : IDrugUsageRepository
{
    private readonly ILogger<DrugUsageRepository> _logger = logger;
    private readonly IDrugUsageStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DrugUsage.DrugUsage>?> GetByFilterCriteriaAsync(DrugUsageFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting drug usages by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} drug usages.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugUsage.DrugUsage?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting drug usage by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved drug usage with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DrugUsage.DrugUsage?> AddAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);

        _logger.LogDebug("Repository: Adding drug usage for DrugId: {DrugId}.", drugUsage.DrugId);

        var result = await _storageClient.AddAsync(drugUsage, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added drug usage for DrugId: {DrugId}.", drugUsage.DrugId);

        return result;
    }

    public async Task<Common.DrugUsage.DrugUsage?> UpdateAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);

        _logger.LogDebug("Repository: Updating drug usage with id: {Id}.", drugUsage.Id);

        var result = await _storageClient.UpdateAsync(drugUsage, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated drug usage with id: {Id}.", drugUsage.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing drug usage with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed drug usage with id: {Id}.", id);
    }
}
