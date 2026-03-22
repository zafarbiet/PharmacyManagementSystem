namespace PharmacyManagementSystem.Server.Unit.QuotationRequest.Data;

public static class SaveQuotationRequestActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.QuotationRequest.QuotationRequest { Status = "Pending", RequestedBy = "user1", RequestDate = DateTimeOffset.UtcNow },
            new Common.QuotationRequest.QuotationRequest { Id = Guid.NewGuid(), Status = "Pending", RequestedBy = "user1", RequestDate = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.QuotationRequest.QuotationRequest { Status = string.Empty, RequestedBy = "user1" }
        };

        yield return new object[]
        {
            new Common.QuotationRequest.QuotationRequest { Status = "Pending", RequestedBy = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            new Common.QuotationRequest.QuotationRequest { Id = id, Status = "Approved", RequestedBy = "user1", RequestDate = DateTimeOffset.UtcNow },
            new Common.QuotationRequest.QuotationRequest { Id = id, Status = "Approved", RequestedBy = "user1", RequestDate = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
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
