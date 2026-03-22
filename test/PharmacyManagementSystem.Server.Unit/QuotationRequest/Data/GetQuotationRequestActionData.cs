using PharmacyManagementSystem.Common.QuotationRequest;

namespace PharmacyManagementSystem.Server.Unit.QuotationRequest.Data;

public static class GetQuotationRequestActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new QuotationRequestFilter { Status = "Pending" },
            new List<Common.QuotationRequest.QuotationRequest>
            {
                new() { Id = Guid.NewGuid(), Status = "Pending", RequestedBy = "user1", IsActive = true }
            }
        };

        yield return new object[]
        {
            new QuotationRequestFilter(),
            new List<Common.QuotationRequest.QuotationRequest>
            {
                new() { Id = Guid.NewGuid(), Status = "Pending", RequestedBy = "user1", IsActive = true },
                new() { Id = Guid.NewGuid(), Status = "Approved", RequestedBy = "user2", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.QuotationRequest.QuotationRequest { Id = id, Status = "Pending", RequestedBy = "user1", IsActive = true }
        };
    }
}
