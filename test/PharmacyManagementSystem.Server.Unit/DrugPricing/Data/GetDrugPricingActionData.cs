using PharmacyManagementSystem.Common.DrugPricing;

namespace PharmacyManagementSystem.Server.Unit.DrugPricing.Data;

public static class GetDrugPricingActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new DrugPricingFilter { DrugId = drugId },
            new List<Common.DrugPricing.DrugPricing>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, CostPrice = 10.00m, SellingPrice = 15.00m }
            }
        };

        yield return new object[]
        {
            new DrugPricingFilter(),
            new List<Common.DrugPricing.DrugPricing>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, CostPrice = 10.00m, SellingPrice = 15.00m },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), DrugId = drugId, CostPrice = 20.00m, SellingPrice = 30.00m }
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
            new Common.DrugPricing.DrugPricing { Id = id, DrugId = drugId, CostPrice = 10.00m, SellingPrice = 15.00m }
        };
    }
}
