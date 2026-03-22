namespace PharmacyManagementSystem.Server.Unit.Rack.Data;

public static class SaveRackActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var zoneId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Rack.Rack { StorageZoneId = zoneId, Label = "A1", Capacity = 100 },
            new Common.Rack.Rack { Id = Guid.NewGuid(), StorageZoneId = zoneId, Label = "A1", Capacity = 100, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.Rack.Rack { StorageZoneId = Guid.Empty, Label = "A1" }
        };

        yield return new object[]
        {
            new Common.Rack.Rack { StorageZoneId = Guid.NewGuid(), Label = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var zoneId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Rack.Rack { Id = id, StorageZoneId = zoneId, Label = "Updated A1" },
            new Common.Rack.Rack { Id = id, StorageZoneId = zoneId, Label = "Updated A1", IsActive = true, UpdatedBy = "system" }
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
