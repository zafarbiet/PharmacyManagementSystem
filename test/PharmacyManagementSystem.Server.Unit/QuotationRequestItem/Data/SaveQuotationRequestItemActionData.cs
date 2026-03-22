namespace PharmacyManagementSystem.Server.Unit.QuotationRequestItem.Data;

public static class SaveQuotationRequestItemActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var quotationRequestId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.QuotationRequestItem.QuotationRequestItem { QuotationRequestId = quotationRequestId, DrugId = drugId, QuantityRequired = 10 },
            new Common.QuotationRequestItem.QuotationRequestItem { Id = Guid.NewGuid(), QuotationRequestId = quotationRequestId, DrugId = drugId, QuantityRequired = 10, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.QuotationRequestItem.QuotationRequestItem { QuotationRequestId = Guid.Empty, DrugId = drugId, QuantityRequired = 10 }
        };

        yield return new object[]
        {
            new Common.QuotationRequestItem.QuotationRequestItem { QuotationRequestId = Guid.NewGuid(), DrugId = Guid.Empty, QuantityRequired = 10 }
        };

        yield return new object[]
        {
            new Common.QuotationRequestItem.QuotationRequestItem { QuotationRequestId = Guid.NewGuid(), DrugId = drugId, QuantityRequired = 0 }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var quotationRequestId = Guid.NewGuid();
        var drugId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.QuotationRequestItem.QuotationRequestItem { Id = id, QuotationRequestId = quotationRequestId, DrugId = drugId, QuantityRequired = 20 },
            new Common.QuotationRequestItem.QuotationRequestItem { Id = id, QuotationRequestId = quotationRequestId, DrugId = drugId, QuantityRequired = 20, IsActive = true, UpdatedBy = "system" }
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
