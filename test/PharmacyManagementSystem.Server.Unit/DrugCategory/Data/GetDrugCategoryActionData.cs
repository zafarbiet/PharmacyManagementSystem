using PharmacyManagementSystem.Common.DrugCategory;

namespace PharmacyManagementSystem.Server.Unit.DrugCategory.Data;

public static class GetDrugCategoryActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new DrugCategoryFilter { Name = "Antibiotic" },
            new List<Common.DrugCategory.DrugCategory>
            {
                new() { Id = Guid.NewGuid(), Name = "Antibiotic", Description = "Antibiotic drugs", IsActive = true }
            }
        };

        yield return new object[]
        {
            new DrugCategoryFilter(),
            new List<Common.DrugCategory.DrugCategory>
            {
                new() { Id = Guid.NewGuid(), Name = "Antibiotic", Description = "Antibiotic drugs", IsActive = true },
                new() { Id = Guid.NewGuid(), Name = "Analgesic", Description = "Pain relief drugs", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.DrugCategory.DrugCategory { Id = id, Name = "Antibiotic", Description = "Antibiotic drugs", IsActive = true }
        };
    }
}
