namespace PharmacyManagementSystem.Server.Unit.DrugPricing.Data;

public static class SaveDrugPricingActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.DrugPricing.DrugPricing { DrugId = drugId, CostPrice = 10.00m, SellingPrice = 15.00m },
            new Common.DrugPricing.DrugPricing { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, CostPrice = 10.00m, SellingPrice = 15.00m, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.DrugPricing.DrugPricing { DrugId = Guid.Empty, CostPrice = 10.00m, SellingPrice = 15.00m }
        };

        yield return new object[]
        {
            new Common.DrugPricing.DrugPricing { DrugId = drugId, CostPrice = 0m, SellingPrice = 15.00m }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.DrugPricing.DrugPricing { Id = id, DrugId = drugId, CostPrice = 12.00m, SellingPrice = 18.00m },
            new Common.DrugPricing.DrugPricing { Id = id, DrugId = drugId, CostPrice = 12.00m, SellingPrice = 18.00m, UpdatedBy = "system" }
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
