using PharmacyManagementSystem.Common.DrugUsage;

namespace PharmacyManagementSystem.Server.Unit.DrugUsage.Data;

public static class GetDrugUsageActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new DrugUsageFilter { DrugId = drugId },
            new List<Common.DrugUsage.DrugUsage>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, DosageInstructions = "Take twice daily" }
            }
        };

        yield return new object[]
        {
            new DrugUsageFilter(),
            new List<Common.DrugUsage.DrugUsage>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, DosageInstructions = "Take twice daily" },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), DrugId = drugId, DosageInstructions = "Take once daily" }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            id.ToString(),
            new Common.DrugUsage.DrugUsage { Id = id, DrugId = drugId, DosageInstructions = "Take twice daily" }
        };
    }
}
