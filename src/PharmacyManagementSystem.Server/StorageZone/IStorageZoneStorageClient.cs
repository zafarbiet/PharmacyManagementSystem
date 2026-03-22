using PharmacyManagementSystem.Common.StorageZone;

namespace PharmacyManagementSystem.Server.StorageZone;

public interface IStorageZoneStorageClient
{
    Task<IReadOnlyCollection<Common.StorageZone.StorageZone>?> GetByFilterCriteriaAsync(StorageZoneFilter filter, CancellationToken cancellationToken);
    Task<Common.StorageZone.StorageZone?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.StorageZone.StorageZone?> AddAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken);
    Task<Common.StorageZone.StorageZone?> UpdateAsync(Common.StorageZone.StorageZone? storageZone, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
