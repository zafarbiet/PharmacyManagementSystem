using PharmacyManagementSystem.Common.QuotationItem;

namespace PharmacyManagementSystem.Server.Unit.QuotationItem.Data;

public static class GetQuotationItemActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var quotationId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new QuotationItemFilter { QuotationId = quotationId },
            new List<Common.QuotationItem.QuotationItem>
            {
                new() { Id = Guid.NewGuid(), QuotationId = quotationId, DrugId = drugId, QuantityOffered = 10, UnitPrice = 5.50m, IsActive = true }
            }
        };

        yield return new object[]
        {
            new QuotationItemFilter(),
            new List<Common.QuotationItem.QuotationItem>
            {
                new() { Id = Guid.NewGuid(), QuotationId = quotationId, DrugId = drugId, QuantityOffered = 10, UnitPrice = 5.50m, IsActive = true },
                new() { Id = Guid.NewGuid(), QuotationId = quotationId, DrugId = Guid.NewGuid(), QuantityOffered = 5, UnitPrice = 3.00m, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        var quotationId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.QuotationItem.QuotationItem { Id = id, QuotationId = quotationId, DrugId = drugId, QuantityOffered = 10, UnitPrice = 5.50m, IsActive = true }
        };
    }
}
