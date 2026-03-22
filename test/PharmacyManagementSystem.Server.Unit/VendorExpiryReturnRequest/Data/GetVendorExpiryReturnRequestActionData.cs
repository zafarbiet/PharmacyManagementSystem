using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;

namespace PharmacyManagementSystem.Server.Unit.VendorExpiryReturnRequest.Data;

public static class GetVendorExpiryReturnRequestActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var expiryRecordId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new VendorExpiryReturnRequestFilter { ExpiryRecordId = expiryRecordId },
            new List<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>
            {
                new() { Id = Guid.NewGuid(), ExpiryRecordId = expiryRecordId, VendorId = vendorId, Status = "Pending", QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow, IsActive = true }
            }
        };

        yield return new object[]
        {
            new VendorExpiryReturnRequestFilter(),
            new List<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>
            {
                new() { Id = Guid.NewGuid(), ExpiryRecordId = expiryRecordId, VendorId = vendorId, Status = "Pending", QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow, IsActive = true },
                new() { Id = Guid.NewGuid(), ExpiryRecordId = Guid.NewGuid(), VendorId = Guid.NewGuid(), Status = "Approved", QuantityToReturn = 5, RequestedAt = DateTimeOffset.UtcNow, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { Id = id, ExpiryRecordId = Guid.NewGuid(), VendorId = Guid.NewGuid(), Status = "Pending", QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow, IsActive = true }
        };
    }
}
