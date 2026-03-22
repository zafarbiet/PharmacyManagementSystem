using PharmacyManagementSystem.Common.StorageZone;

namespace PharmacyManagementSystem.Server.StorageZone;

public interface IGetStorageZoneAction
{
    Task<IReadOnlyCollection<Common.StorageZone.StorageZone>?> GetByFilterCriteriaAsync(StorageZoneFilter filter, CancellationToken cancellationToken);
    Task<Common.StorageZone.StorageZone?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
