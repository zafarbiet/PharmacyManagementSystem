using PharmacyManagementSystem.Common.Vendor;

namespace PharmacyManagementSystem.Server.Unit.Vendor.Data;

public static class GetVendorActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new VendorFilter { Name = "MediSupply" },
            new List<Common.Vendor.Vendor>
            {
                new() { Id = Guid.NewGuid(), Name = "MediSupply", ContactPerson = "John Doe", Phone = "1234567890", IsActive = true }
            }
        };

        yield return new object[]
        {
            new VendorFilter(),
            new List<Common.Vendor.Vendor>
            {
                new() { Id = Guid.NewGuid(), Name = "MediSupply", ContactPerson = "John Doe", Phone = "1234567890", IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "PharmaDist", ContactPerson = "Jane Smith", Phone = "0987654321", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.Vendor.Vendor { Id = id, Name = "MediSupply", ContactPerson = "John Doe", IsActive = true }
        };
    }
}
