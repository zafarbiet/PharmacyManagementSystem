namespace PharmacyManagementSystem.Server.Unit.Drug.Data;

public static class SaveDrugActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var categoryId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Drug.Drug { Name = "Amoxicillin", GenericName = "Amoxicillin", CategoryId = categoryId, ReorderLevel = 10 },
            new Common.Drug.Drug { Id = Guid.NewGuid(), Name = "Amoxicillin", GenericName = "Amoxicillin", CategoryId = categoryId, ReorderLevel = 10, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var categoryId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Drug.Drug { Name = string.Empty, CategoryId = categoryId }
        };

        yield return new object[]
        {
            new Common.Drug.Drug { Name = "ValidName", CategoryId = Guid.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Drug.Drug { Id = id, Name = "Updated Amoxicillin", GenericName = "Amoxicillin", CategoryId = categoryId },
            new Common.Drug.Drug { Id = id, Name = "Updated Amoxicillin", GenericName = "Amoxicillin", CategoryId = categoryId, IsActive = true, UpdatedBy = "system" }
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
