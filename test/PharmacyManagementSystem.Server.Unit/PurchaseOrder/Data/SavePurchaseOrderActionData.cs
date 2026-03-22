namespace PharmacyManagementSystem.Server.Unit.PurchaseOrder.Data;

public static class SavePurchaseOrderActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var vendorId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.PurchaseOrder.PurchaseOrder { VendorId = vendorId, Status = "Pending", TotalAmount = 500.00m },
            new Common.PurchaseOrder.PurchaseOrder { Id = new Guid("11111111-1111-1111-1111-111111111111"), VendorId = vendorId, Status = "Pending", TotalAmount = 500.00m, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var vendorId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.PurchaseOrder.PurchaseOrder { VendorId = Guid.Empty, Status = "Pending" }
        };

        yield return new object[]
        {
            new Common.PurchaseOrder.PurchaseOrder { VendorId = vendorId, Status = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var vendorId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.PurchaseOrder.PurchaseOrder { Id = id, VendorId = vendorId, Status = "Received", TotalAmount = 500.00m },
            new Common.PurchaseOrder.PurchaseOrder { Id = id, VendorId = vendorId, Status = "Received", TotalAmount = 500.00m, UpdatedBy = "system" }
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
