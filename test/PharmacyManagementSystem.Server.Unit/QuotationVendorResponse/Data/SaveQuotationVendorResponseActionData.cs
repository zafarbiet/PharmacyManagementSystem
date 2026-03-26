namespace PharmacyManagementSystem.Server.Unit.QuotationVendorResponse.Data;

public static class SaveQuotationVendorResponseActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var requestId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.QuotationVendorResponse.QuotationVendorResponse { QuotationRequestId = requestId, VendorId = vendorId },
            new Common.QuotationVendorResponse.QuotationVendorResponse { Id = Guid.NewGuid(), QuotationRequestId = requestId, VendorId = vendorId, Status = "pending", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.QuotationVendorResponse.QuotationVendorResponse { QuotationRequestId = Guid.Empty, VendorId = Guid.NewGuid() }
        };

        yield return new object[]
        {
            new Common.QuotationVendorResponse.QuotationVendorResponse { QuotationRequestId = Guid.NewGuid(), VendorId = Guid.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.QuotationVendorResponse.QuotationVendorResponse { Id = id, QuotationRequestId = requestId, VendorId = vendorId, Status = "accepted" },
            new Common.QuotationVendorResponse.QuotationVendorResponse { Id = id, QuotationRequestId = requestId, VendorId = vendorId, Status = "accepted", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
