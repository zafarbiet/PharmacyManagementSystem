using PharmacyManagementSystem.Common.PurchaseOrder;

namespace PharmacyManagementSystem.Server.Unit.PurchaseOrder.Data;

public static class GetPurchaseOrderActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var vendorId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new PurchaseOrderFilter { VendorId = vendorId },
            new List<Common.PurchaseOrder.PurchaseOrder>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), VendorId = vendorId, Status = "Pending", TotalAmount = 500.00m }
            }
        };

        yield return new object[]
        {
            new PurchaseOrderFilter(),
            new List<Common.PurchaseOrder.PurchaseOrder>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), VendorId = vendorId, Status = "Pending", TotalAmount = 500.00m },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), VendorId = vendorId, Status = "Received", TotalAmount = 1000.00m }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var vendorId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            id.ToString(),
            new Common.PurchaseOrder.PurchaseOrder { Id = id, VendorId = vendorId, Status = "Pending", TotalAmount = 500.00m }
        };
    }
}
