using PharmacyManagementSystem.Common.QuotationVendorResponse;

namespace PharmacyManagementSystem.Server.Unit.QuotationVendorResponse.Data;

public static class GetQuotationVendorResponseActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var requestId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();

        yield return new object[]
        {
            new QuotationVendorResponseFilter { QuotationRequestId = requestId },
            new List<Common.QuotationVendorResponse.QuotationVendorResponse>
            {
                new() { Id = Guid.NewGuid(), QuotationRequestId = requestId, VendorId = vendorId, Status = "pending", IsActive = true }
            }
        };

        yield return new object[]
        {
            new QuotationVendorResponseFilter(),
            new List<Common.QuotationVendorResponse.QuotationVendorResponse>
            {
                new() { Id = Guid.NewGuid(), QuotationRequestId = requestId, VendorId = vendorId, Status = "pending", IsActive = true },
                new() { Id = Guid.NewGuid(), QuotationRequestId = Guid.NewGuid(), VendorId = Guid.NewGuid(), Status = "accepted", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.QuotationVendorResponse.QuotationVendorResponse { Id = id, QuotationRequestId = Guid.NewGuid(), VendorId = Guid.NewGuid(), Status = "pending", IsActive = true }
        };
    }
}
