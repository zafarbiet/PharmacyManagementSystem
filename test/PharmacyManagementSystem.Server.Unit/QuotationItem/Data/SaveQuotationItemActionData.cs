namespace PharmacyManagementSystem.Server.Unit.QuotationItem.Data;

public static class SaveQuotationItemActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var quotationId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.QuotationItem.QuotationItem { QuotationId = quotationId, DrugId = drugId, QuantityOffered = 10, UnitPrice = 5.50m },
            new Common.QuotationItem.QuotationItem { Id = Guid.NewGuid(), QuotationId = quotationId, DrugId = drugId, QuantityOffered = 10, UnitPrice = 5.50m, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.QuotationItem.QuotationItem { QuotationId = Guid.Empty, DrugId = drugId, QuantityOffered = 10, UnitPrice = 5.50m }
        };

        yield return new object[]
        {
            new Common.QuotationItem.QuotationItem { QuotationId = Guid.NewGuid(), DrugId = Guid.Empty, QuantityOffered = 10, UnitPrice = 5.50m }
        };

        yield return new object[]
        {
            new Common.QuotationItem.QuotationItem { QuotationId = Guid.NewGuid(), DrugId = drugId, QuantityOffered = 0, UnitPrice = 5.50m }
        };

        yield return new object[]
        {
            new Common.QuotationItem.QuotationItem { QuotationId = Guid.NewGuid(), DrugId = drugId, QuantityOffered = 10, UnitPrice = 0m }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var quotationId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.QuotationItem.QuotationItem { Id = id, QuotationId = quotationId, DrugId = drugId, QuantityOffered = 20, UnitPrice = 6.00m },
            new Common.QuotationItem.QuotationItem { Id = id, QuotationId = quotationId, DrugId = drugId, QuantityOffered = 20, UnitPrice = 6.00m, IsActive = true, UpdatedBy = "system" }
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
