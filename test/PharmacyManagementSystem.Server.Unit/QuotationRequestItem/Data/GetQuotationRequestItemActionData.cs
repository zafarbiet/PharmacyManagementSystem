using PharmacyManagementSystem.Common.QuotationRequestItem;

namespace PharmacyManagementSystem.Server.Unit.QuotationRequestItem.Data;

public static class GetQuotationRequestItemActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var quotationRequestId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new QuotationRequestItemFilter { QuotationRequestId = quotationRequestId },
            new List<Common.QuotationRequestItem.QuotationRequestItem>
            {
                new() { Id = Guid.NewGuid(), QuotationRequestId = quotationRequestId, DrugId = drugId, QuantityRequired = 10, IsActive = true }
            }
        };

        yield return new object[]
        {
            new QuotationRequestItemFilter(),
            new List<Common.QuotationRequestItem.QuotationRequestItem>
            {
                new() { Id = Guid.NewGuid(), QuotationRequestId = quotationRequestId, DrugId = drugId, QuantityRequired = 10, IsActive = true },
                new() { Id = Guid.NewGuid(), QuotationRequestId = quotationRequestId, DrugId = Guid.NewGuid(), QuantityRequired = 5, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        var quotationRequestId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.QuotationRequestItem.QuotationRequestItem { Id = id, QuotationRequestId = quotationRequestId, DrugId = drugId, QuantityRequired = 10, IsActive = true }
        };
    }
}
