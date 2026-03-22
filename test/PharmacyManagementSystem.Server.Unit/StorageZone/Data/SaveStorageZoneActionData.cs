namespace PharmacyManagementSystem.Server.Unit.StorageZone.Data;

public static class SaveStorageZoneActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.StorageZone.StorageZone { Name = "Zone A", ZoneType = "Cold" },
            new Common.StorageZone.StorageZone { Id = Guid.NewGuid(), Name = "Zone A", ZoneType = "Cold", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.StorageZone.StorageZone { Name = string.Empty, ZoneType = "Cold" }
        };

        yield return new object[]
        {
            new Common.StorageZone.StorageZone { Name = "Zone A", ZoneType = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.StorageZone.StorageZone { Id = id, Name = "Updated Zone A", ZoneType = "Ambient" },
            new Common.StorageZone.StorageZone { Id = id, Name = "Updated Zone A", ZoneType = "Ambient", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[]
        {
            Guid.NewGuid(),
            "system"
        };
    }
}
