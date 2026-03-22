namespace PharmacyManagementSystem.Server.Unit.Vendor.Data;

public static class SaveVendorActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.Vendor.Vendor { Name = "MediSupply", ContactPerson = "John Doe", Phone = "1234567890", Email = "john@medisupply.com" },
            new Common.Vendor.Vendor { Id = Guid.NewGuid(), Name = "MediSupply", ContactPerson = "John Doe", Phone = "1234567890", Email = "john@medisupply.com", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.Vendor.Vendor { Name = string.Empty, ContactPerson = "John Doe" }
        };

        yield return new object[]
        {
            new Common.Vendor.Vendor { Name = "   ", ContactPerson = "Whitespace name" }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Vendor.Vendor { Id = id, Name = "Updated MediSupply", ContactPerson = "John Doe Updated" },
            new Common.Vendor.Vendor { Id = id, Name = "Updated MediSupply", ContactPerson = "John Doe Updated", IsActive = true, UpdatedBy = "system" }
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
