using PharmacyManagementSystem.Common.DrugInventory;

namespace PharmacyManagementSystem.Server.Unit.DrugInventory.Data;

public static class GetDrugInventoryActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new DrugInventoryFilter { DrugId = drugId },
            new List<Common.DrugInventory.DrugInventory>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, BatchNumber = "BATCH001", QuantityInStock = 100 }
            }
        };

        yield return new object[]
        {
            new DrugInventoryFilter(),
            new List<Common.DrugInventory.DrugInventory>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, BatchNumber = "BATCH001", QuantityInStock = 100 },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), DrugId = drugId, BatchNumber = "BATCH002", QuantityInStock = 50 }
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
            new Common.DrugInventory.DrugInventory { Id = id, DrugId = drugId, BatchNumber = "BATCH001", QuantityInStock = 100 }
        };
    }
}
