namespace PharmacyManagementSystem.Server.StorageZone;

public interface ISaveStorageZoneAction
{
    Task<Common.StorageZone.StorageZone?> AddAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken);
    Task<Common.StorageZone.StorageZone?> UpdateAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
