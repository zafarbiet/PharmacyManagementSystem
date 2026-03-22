namespace PharmacyManagementSystem.Server.Unit.DrugUsage.Data;

public static class SaveDrugUsageActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.DrugUsage.DrugUsage { DrugId = drugId, DosageInstructions = "Take twice daily" },
            new Common.DrugUsage.DrugUsage { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, DosageInstructions = "Take twice daily", UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DrugUsage.DrugUsage { DrugId = Guid.Empty, DosageInstructions = "Take twice daily" }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.DrugUsage.DrugUsage { Id = id, DrugId = drugId, DosageInstructions = "Take three times daily" },
            new Common.DrugUsage.DrugUsage { Id = id, DrugId = drugId, DosageInstructions = "Take three times daily", UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[]
        {
            new Guid("11111111-1111-1111-1111-111111111111"),
            "system"
        };
    }
}
