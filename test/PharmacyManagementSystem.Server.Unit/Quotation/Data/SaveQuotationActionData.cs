namespace PharmacyManagementSystem.Server.Unit.Quotation.Data;

public static class SaveQuotationActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var quotationRequestId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Quotation.Quotation { QuotationRequestId = quotationRequestId, VendorId = vendorId, Status = "Submitted", QuotationDate = DateTimeOffset.UtcNow },
            new Common.Quotation.Quotation { Id = Guid.NewGuid(), QuotationRequestId = quotationRequestId, VendorId = vendorId, Status = "Submitted", QuotationDate = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Quotation.Quotation { QuotationRequestId = Guid.Empty, VendorId = vendorId, Status = "Submitted" }
        };

        yield return new object[]
        {
            new Common.Quotation.Quotation { QuotationRequestId = Guid.NewGuid(), VendorId = Guid.Empty, Status = "Submitted" }
        };

        yield return new object[]
        {
            new Common.Quotation.Quotation { QuotationRequestId = Guid.NewGuid(), VendorId = vendorId, Status = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var quotationRequestId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.Quotation.Quotation { Id = id, QuotationRequestId = quotationRequestId, VendorId = vendorId, Status = "Approved", QuotationDate = DateTimeOffset.UtcNow },
            new Common.Quotation.Quotation { Id = id, QuotationRequestId = quotationRequestId, VendorId = vendorId, Status = "Approved", QuotationDate = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
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
