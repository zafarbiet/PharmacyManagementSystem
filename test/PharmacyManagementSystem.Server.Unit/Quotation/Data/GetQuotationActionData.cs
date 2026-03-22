using PharmacyManagementSystem.Common.Quotation;

namespace PharmacyManagementSystem.Server.Unit.Quotation.Data;

public static class GetQuotationActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new QuotationFilter { Status = "Submitted" },
            new List<Common.Quotation.Quotation>
            {
                new() { Id = Guid.NewGuid(), VendorId = vendorId, Status = "Submitted", QuotationDate = DateTimeOffset.UtcNow, IsActive = true }
            }
        };

        yield return new object[]
        {
            new QuotationFilter(),
            new List<Common.Quotation.Quotation>
            {
                new() { Id = Guid.NewGuid(), VendorId = vendorId, Status = "Submitted", QuotationDate = DateTimeOffset.UtcNow, IsActive = true },
                new() { Id = Guid.NewGuid(), VendorId = Guid.NewGuid(), Status = "Approved", QuotationDate = DateTimeOffset.UtcNow, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.Quotation.Quotation { Id = id, VendorId = vendorId, Status = "Submitted", QuotationDate = DateTimeOffset.UtcNow, IsActive = true }
        };
    }
}
