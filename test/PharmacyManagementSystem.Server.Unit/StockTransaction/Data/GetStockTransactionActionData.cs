using PharmacyManagementSystem.Common.StockTransaction;

namespace PharmacyManagementSystem.Server.Unit.StockTransaction.Data;

public static class GetStockTransactionActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var drugId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new StockTransactionFilter { DrugId = drugId },
            new List<Common.StockTransaction.StockTransaction>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, TransactionType = "IN", Quantity = 100 }
            }
        };

        yield return new object[]
        {
            new StockTransactionFilter(),
            new List<Common.StockTransaction.StockTransaction>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), DrugId = drugId, TransactionType = "IN", Quantity = 100 },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), DrugId = drugId, TransactionType = "OUT", Quantity = 10 }
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
            new Common.StockTransaction.StockTransaction { Id = id, DrugId = drugId, TransactionType = "IN", Quantity = 100 }
        };
    }
}
