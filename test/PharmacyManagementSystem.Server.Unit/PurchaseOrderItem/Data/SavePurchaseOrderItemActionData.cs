namespace PharmacyManagementSystem.Server.Unit.PurchaseOrderItem.Data;

public static class SavePurchaseOrderItemActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var purchaseOrderId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            new Common.PurchaseOrderItem.PurchaseOrderItem { PurchaseOrderId = purchaseOrderId, DrugId = drugId, QuantityOrdered = 100, UnitPrice = 5.00m },
            new Common.PurchaseOrderItem.PurchaseOrderItem { Id = new Guid("11111111-1111-1111-1111-111111111111"), PurchaseOrderId = purchaseOrderId, DrugId = drugId, QuantityOrdered = 100, UnitPrice = 5.00m, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        var purchaseOrderId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.PurchaseOrderItem.PurchaseOrderItem { PurchaseOrderId = Guid.Empty, DrugId = drugId, QuantityOrdered = 100 }
        };

        yield return new object[]
        {
            new Common.PurchaseOrderItem.PurchaseOrderItem { PurchaseOrderId = purchaseOrderId, DrugId = Guid.Empty, QuantityOrdered = 100 }
        };

        yield return new object[]
        {
            new Common.PurchaseOrderItem.PurchaseOrderItem { PurchaseOrderId = purchaseOrderId, DrugId = drugId, QuantityOrdered = 0 }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var purchaseOrderId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            new Common.PurchaseOrderItem.PurchaseOrderItem { Id = id, PurchaseOrderId = purchaseOrderId, DrugId = drugId, QuantityOrdered = 200, UnitPrice = 5.00m },
            new Common.PurchaseOrderItem.PurchaseOrderItem { Id = id, PurchaseOrderId = purchaseOrderId, DrugId = drugId, QuantityOrdered = 200, UnitPrice = 5.00m, UpdatedBy = "system" }
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
