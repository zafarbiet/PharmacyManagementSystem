using PharmacyManagementSystem.Common.Rack;

namespace PharmacyManagementSystem.Server.Unit.Rack.Data;

public static class GetRackActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var zoneId = Guid.NewGuid();
        yield return new object[]
        {
            new RackFilter { StorageZoneId = zoneId },
            new List<Common.Rack.Rack>
            {
                new() { Id = Guid.NewGuid(), StorageZoneId = zoneId, Label = "A1", IsActive = true }
            }
        };

        yield return new object[]
        {
            new RackFilter(),
            new List<Common.Rack.Rack>
            {
                new() { Id = Guid.NewGuid(), StorageZoneId = zoneId, Label = "A1", IsActive = true },
                new() { Id = Guid.NewGuid(), StorageZoneId = Guid.NewGuid(), Label = "B2", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.Rack.Rack { Id = id, StorageZoneId = Guid.NewGuid(), Label = "A1", IsActive = true }
        };
    }
}
