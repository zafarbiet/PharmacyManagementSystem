using PharmacyManagementSystem.Common.Manufacturer;

namespace PharmacyManagementSystem.Server.Unit.Manufacturer.Data;

public static class GetManufacturerActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new ManufacturerFilter { Name = "Sun Pharma" },
            new List<Common.Manufacturer.Manufacturer>
            {
                new() { Id = Guid.NewGuid(), Name = "Sun Pharma", Country = "India", IsActive = true }
            }
        };

        yield return new object[]
        {
            new ManufacturerFilter(),
            new List<Common.Manufacturer.Manufacturer>
            {
                new() { Id = Guid.NewGuid(), Name = "Sun Pharma", Country = "India", IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "Cipla Ltd", Country = "India", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.Manufacturer.Manufacturer { Id = id, Name = "Sun Pharma", Country = "India", IsActive = true }
        };
    }
}
