namespace PharmacyManagementSystem.Server.Unit.StockTransaction.Data;

public static class SaveStockTransactionActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.StockTransaction.StockTransaction { DrugId = drugId, TransactionType = "IN", Quantity = 100 },
            new Common.StockTransaction.StockTransaction { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, TransactionType = "IN", Quantity = 100, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.StockTransaction.StockTransaction { DrugId = Guid.Empty, TransactionType = "IN", Quantity = 100 }
        };

        yield return new object[]
        {
            new Common.StockTransaction.StockTransaction { DrugId = drugId, TransactionType = string.Empty, Quantity = 100 }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.StockTransaction.StockTransaction { Id = id, DrugId = drugId, TransactionType = "OUT", Quantity = 10 },
            new Common.StockTransaction.StockTransaction { Id = id, DrugId = drugId, TransactionType = "OUT", Quantity = 10, UpdatedBy = "system" }
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
