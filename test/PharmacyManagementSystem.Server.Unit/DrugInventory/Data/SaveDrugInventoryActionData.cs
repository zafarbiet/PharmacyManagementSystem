namespace PharmacyManagementSystem.Server.Unit.DrugInventory.Data;

public static class SaveDrugInventoryActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.DrugInventory.DrugInventory { DrugId = drugId, BatchNumber = "BATCH001", QuantityInStock = 100 },
            new Common.DrugInventory.DrugInventory { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, BatchNumber = "BATCH001", QuantityInStock = 100, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.DrugInventory.DrugInventory { DrugId = Guid.Empty, BatchNumber = "BATCH001", QuantityInStock = 100 }
        };

        yield return new object[]
        {
            new Common.DrugInventory.DrugInventory { DrugId = drugId, BatchNumber = string.Empty, QuantityInStock = 100 }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.DrugInventory.DrugInventory { Id = id, DrugId = drugId, BatchNumber = "BATCH001-UPD", QuantityInStock = 200 },
            new Common.DrugInventory.DrugInventory { Id = id, DrugId = drugId, BatchNumber = "BATCH001-UPD", QuantityInStock = 200, UpdatedBy = "system" }
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
