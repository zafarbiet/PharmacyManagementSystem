using PharmacyManagementSystem.Common.Drug;

namespace PharmacyManagementSystem.Server.Unit.Drug.Data;

public static class GetDrugActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var categoryId = Guid.NewGuid();
        yield return new object[]
        {
            new DrugFilter { Name = "Amoxicillin" },
            new List<Common.Drug.Drug>
            {
                new() { Id = Guid.NewGuid(), Name = "Amoxicillin", GenericName = "Amoxicillin", CategoryId = categoryId, IsActive = true }
            }
        };

        yield return new object[]
        {
            new DrugFilter(),
            new List<Common.Drug.Drug>
            {
                new() { Id = Guid.NewGuid(), Name = "Amoxicillin", GenericName = "Amoxicillin", CategoryId = categoryId, IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "Ibuprofen", GenericName = "Ibuprofen", CategoryId = categoryId, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.Drug.Drug { Id = id, Name = "Amoxicillin", GenericName = "Amoxicillin", CategoryId = categoryId, IsActive = true }
        };
    }
}
