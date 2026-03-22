using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.StorageZone;

public class SaveStorageZoneAction(ILogger<SaveStorageZoneAction> logger, IStorageZoneRepository repository) : ISaveStorageZoneAction
{
    private readonly ILogger<SaveStorageZoneAction> _logger = logger;
    private readonly IStorageZoneRepository _repository = repository;

    public async Task<Common.StorageZone.StorageZone?> AddAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(storageZone);

        if (string.IsNullOrWhiteSpace(storageZone.Name))
            throw new BadRequestException("StorageZone Name is required.");

        if (string.IsNullOrWhiteSpace(storageZone.ZoneType))
            throw new BadRequestException("StorageZone ZoneType is required.");

        storageZone.UpdatedBy = "system";

        _logger.LogDebug("Adding new storage zone with name: {Name}.", storageZone.Name);

        var result = await _repository.AddAsync(storageZone, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added storage zone with name: {Name}.", storageZone.Name);

        return result;
    }

    public async Task<Common.StorageZone.StorageZone?> UpdateAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(storageZone);

        if (string.IsNullOrWhiteSpace(storageZone.Name))
            throw new BadRequestException("StorageZone Name is required.");

        if (string.IsNullOrWhiteSpace(storageZone.ZoneType))
            throw new BadRequestException("StorageZone ZoneType is required.");

        storageZone.UpdatedBy = "system";

        _logger.LogDebug("Updating storage zone with id: {Id}.", storageZone.Id);

        var result = await _repository.UpdateAsync(storageZone, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated storage zone with id: {Id}.", storageZone.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing storage zone with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed storage zone with id: {Id}.", id);
    }
}
