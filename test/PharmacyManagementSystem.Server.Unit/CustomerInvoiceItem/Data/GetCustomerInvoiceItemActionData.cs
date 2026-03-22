using PharmacyManagementSystem.Common.CustomerInvoiceItem;

namespace PharmacyManagementSystem.Server.Unit.CustomerInvoiceItem.Data;

public static class GetCustomerInvoiceItemActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var invoiceId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            new CustomerInvoiceItemFilter { InvoiceId = invoiceId },
            new List<Common.CustomerInvoiceItem.CustomerInvoiceItem>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), InvoiceId = invoiceId, DrugId = drugId, Quantity = 2, UnitPrice = 15.00m, Amount = 30.00m }
            }
        };

        yield return new object[]
        {
            new CustomerInvoiceItemFilter(),
            new List<Common.CustomerInvoiceItem.CustomerInvoiceItem>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), InvoiceId = invoiceId, DrugId = drugId, Quantity = 2, UnitPrice = 15.00m, Amount = 30.00m },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), InvoiceId = invoiceId, DrugId = drugId, Quantity = 1, UnitPrice = 20.00m, Amount = 20.00m }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var invoiceId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            id.ToString(),
            new Common.CustomerInvoiceItem.CustomerInvoiceItem { Id = id, InvoiceId = invoiceId, DrugId = drugId, Quantity = 2, UnitPrice = 15.00m, Amount = 30.00m }
        };
    }
}
