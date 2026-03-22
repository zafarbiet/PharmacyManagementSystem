namespace PharmacyManagementSystem.Server.Unit.VendorExpiryReturnRequest.Data;

public static class SaveVendorExpiryReturnRequestActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var expiryRecordId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { ExpiryRecordId = expiryRecordId, VendorId = vendorId, Status = "Pending", QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow },
            new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { Id = Guid.NewGuid(), ExpiryRecordId = expiryRecordId, VendorId = vendorId, Status = "Pending", QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { ExpiryRecordId = Guid.Empty, VendorId = Guid.NewGuid(), Status = "Pending", QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow }
        };

        yield return new object[]
        {
            new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { ExpiryRecordId = Guid.NewGuid(), VendorId = Guid.Empty, Status = "Pending", QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow }
        };

        yield return new object[]
        {
            new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { ExpiryRecordId = Guid.NewGuid(), VendorId = Guid.NewGuid(), Status = string.Empty, QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow }
        };

        yield return new object[]
        {
            new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { ExpiryRecordId = Guid.NewGuid(), VendorId = Guid.NewGuid(), Status = "Pending", QuantityToReturn = 0, RequestedAt = DateTimeOffset.UtcNow }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var expiryRecordId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { Id = id, ExpiryRecordId = expiryRecordId, VendorId = vendorId, Status = "Approved", QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow },
            new Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest { Id = id, ExpiryRecordId = expiryRecordId, VendorId = vendorId, Status = "Approved", QuantityToReturn = 10, RequestedAt = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[]
        {
            Guid.NewGuid(),
            "system"
        };
    }
}
