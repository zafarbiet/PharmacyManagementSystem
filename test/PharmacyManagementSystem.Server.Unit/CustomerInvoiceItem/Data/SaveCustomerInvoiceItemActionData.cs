namespace PharmacyManagementSystem.Server.Unit.CustomerInvoiceItem.Data;

public static class SaveCustomerInvoiceItemActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var invoiceId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            new Common.CustomerInvoiceItem.CustomerInvoiceItem { InvoiceId = invoiceId, DrugId = drugId, Quantity = 2, UnitPrice = 15.00m, Amount = 30.00m },
            new Common.CustomerInvoiceItem.CustomerInvoiceItem { Id = new Guid("11111111-1111-1111-1111-111111111111"), InvoiceId = invoiceId, DrugId = drugId, Quantity = 2, UnitPrice = 15.00m, Amount = 30.00m, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        var invoiceId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.CustomerInvoiceItem.CustomerInvoiceItem { InvoiceId = Guid.Empty, DrugId = drugId, Quantity = 2 }
        };

        yield return new object[]
        {
            new Common.CustomerInvoiceItem.CustomerInvoiceItem { InvoiceId = invoiceId, DrugId = Guid.Empty, Quantity = 2 }
        };

        yield return new object[]
        {
            new Common.CustomerInvoiceItem.CustomerInvoiceItem { InvoiceId = invoiceId, DrugId = drugId, Quantity = 0 }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var invoiceId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            new Common.CustomerInvoiceItem.CustomerInvoiceItem { Id = id, InvoiceId = invoiceId, DrugId = drugId, Quantity = 3, UnitPrice = 15.00m, Amount = 45.00m },
            new Common.CustomerInvoiceItem.CustomerInvoiceItem { Id = id, InvoiceId = invoiceId, DrugId = drugId, Quantity = 3, UnitPrice = 15.00m, Amount = 45.00m, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[]
        {
            new Guid("11111111-1111-1111-1111-111111111111"),
            "system"
        };
    }
}
