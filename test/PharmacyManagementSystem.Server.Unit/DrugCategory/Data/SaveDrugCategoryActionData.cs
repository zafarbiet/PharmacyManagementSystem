using PharmacyManagementSystem.Common.DrugCategory;

namespace PharmacyManagementSystem.Server.Unit.DrugCategory.Data;

public static class SaveDrugCategoryActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.DrugCategory.DrugCategory { Name = "Antibiotic", Description = "Antibiotic drugs" },
            new Common.DrugCategory.DrugCategory { Id = Guid.NewGuid(), Name = "Antibiotic", Description = "Antibiotic drugs", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DrugCategory.DrugCategory { Name = string.Empty, Description = "Missing name" }
        };

        yield return new object[]
        {
            new Common.DrugCategory.DrugCategory { Name = "   ", Description = "Whitespace name" }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DrugCategory.DrugCategory { Id = id, Name = "Updated Antibiotic", Description = "Updated description" },
            new Common.DrugCategory.DrugCategory { Id = id, Name = "Updated Antibiotic", Description = "Updated description", IsActive = true, UpdatedBy = "system" }
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
