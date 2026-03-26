namespace PharmacyManagementSystem.Server.Unit.Manufacturer.Data;

public static class SaveManufacturerActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.Manufacturer.Manufacturer { Name = "Sun Pharma", Country = "India" },
            new Common.Manufacturer.Manufacturer { Id = Guid.NewGuid(), Name = "Sun Pharma", Country = "India", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.Manufacturer.Manufacturer { Name = string.Empty, Country = "India" }
        };

        yield return new object[]
        {
            new Common.Manufacturer.Manufacturer { Name = "   ", Country = "India" }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Manufacturer.Manufacturer { Id = id, Name = "Cipla Ltd", Country = "India" },
            new Common.Manufacturer.Manufacturer { Id = id, Name = "Cipla Ltd", Country = "India", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
