using PharmacyManagementSystem.Common.PurchaseOrderItem;

namespace PharmacyManagementSystem.Server.Unit.PurchaseOrderItem.Data;

public static class GetPurchaseOrderItemActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var purchaseOrderId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            new PurchaseOrderItemFilter { PurchaseOrderId = purchaseOrderId },
            new List<Common.PurchaseOrderItem.PurchaseOrderItem>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), PurchaseOrderId = purchaseOrderId, DrugId = drugId, QuantityOrdered = 100, UnitPrice = 5.00m }
            }
        };

        yield return new object[]
        {
            new PurchaseOrderItemFilter(),
            new List<Common.PurchaseOrderItem.PurchaseOrderItem>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), PurchaseOrderId = purchaseOrderId, DrugId = drugId, QuantityOrdered = 100, UnitPrice = 5.00m },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), PurchaseOrderId = purchaseOrderId, DrugId = drugId, QuantityOrdered = 50, UnitPrice = 10.00m }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var purchaseOrderId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            id.ToString(),
            new Common.PurchaseOrderItem.PurchaseOrderItem { Id = id, PurchaseOrderId = purchaseOrderId, DrugId = drugId, QuantityOrdered = 100, UnitPrice = 5.00m }
        };
    }
}
