using PharmacyManagementSystem.Common.StorageZone;

namespace PharmacyManagementSystem.Server.Unit.StorageZone.Data;

public static class GetStorageZoneActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new StorageZoneFilter { Name = "Zone A" },
            new List<Common.StorageZone.StorageZone>
            {
                new() { Id = Guid.NewGuid(), Name = "Zone A", ZoneType = "Cold", IsActive = true }
            }
        };

        yield return new object[]
        {
            new StorageZoneFilter(),
            new List<Common.StorageZone.StorageZone>
            {
                new() { Id = Guid.NewGuid(), Name = "Zone A", ZoneType = "Cold", IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "Zone B", ZoneType = "Ambient", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.StorageZone.StorageZone { Id = id, Name = "Zone A", ZoneType = "Cold", IsActive = true }
        };
    }
}
