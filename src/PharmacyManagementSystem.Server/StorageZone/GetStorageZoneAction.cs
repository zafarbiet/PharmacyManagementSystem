using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.StorageZone;

namespace PharmacyManagementSystem.Server.StorageZone;

public class GetStorageZoneAction(ILogger<GetStorageZoneAction> logger, IStorageZoneRepository repository) : IGetStorageZoneAction
{
    private readonly ILogger<GetStorageZoneAction> _logger = logger;
    private readonly IStorageZoneRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.StorageZone.StorageZone>?> GetByFilterCriteriaAsync(StorageZoneFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting storage zones by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} storage zones.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.StorageZone.StorageZone?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting storage zone by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved storage zone with id: {Id}.", id);

        return result;
    }
}
