using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.StorageZone;

namespace PharmacyManagementSystem.Server.StorageZone;

public class StorageZoneRepository(ILogger<StorageZoneRepository> logger, IStorageZoneStorageClient storageClient) : IStorageZoneRepository
{
    private readonly ILogger<StorageZoneRepository> _logger = logger;
    private readonly IStorageZoneStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.StorageZone.StorageZone>?> GetByFilterCriteriaAsync(StorageZoneFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting storage zones by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} storage zones.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.StorageZone.StorageZone?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting storage zone by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved storage zone with id: {Id}.", id);

        return result;
    }

    public async Task<Common.StorageZone.StorageZone?> AddAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(storageZone);

        _logger.LogDebug("Repository: Adding storage zone with name: {Name}.", storageZone.Name);

        var result = await _storageClient.AddAsync(storageZone, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added storage zone with name: {Name}.", storageZone.Name);

        return result;
    }

    public async Task<Common.StorageZone.StorageZone?> UpdateAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(storageZone);

        _logger.LogDebug("Repository: Updating storage zone with id: {Id}.", storageZone.Id);

        var result = await _storageClient.UpdateAsync(storageZone, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated storage zone with id: {Id}.", storageZone.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing storage zone with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed storage zone with id: {Id}.", id);
    }
}
