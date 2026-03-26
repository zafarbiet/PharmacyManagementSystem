using PharmacyManagementSystem.Common.PaymentLedger;

namespace PharmacyManagementSystem.Server.Unit.PaymentLedger.Data;

public static class GetPaymentLedgerActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var vendorId = Guid.NewGuid();

        yield return new object[]
        {
            new PaymentLedgerFilter { VendorId = vendorId },
            new List<Common.PaymentLedger.PaymentLedger>
            {
                new() { Id = Guid.NewGuid(), VendorId = vendorId, InvoicedAmount = 50000m, PaidAmount = 0m, Status = "Unpaid", IsActive = true }
            }
        };

        yield return new object[]
        {
            new PaymentLedgerFilter(),
            new List<Common.PaymentLedger.PaymentLedger>
            {
                new() { Id = Guid.NewGuid(), VendorId = vendorId, InvoicedAmount = 50000m, PaidAmount = 0m, Status = "Unpaid", IsActive = true },
                new() { Id = Guid.NewGuid(), VendorId = Guid.NewGuid(), InvoicedAmount = 20000m, PaidAmount = 20000m, Status = "Paid", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.PaymentLedger.PaymentLedger { Id = id, VendorId = Guid.NewGuid(), InvoicedAmount = 50000m, PaidAmount = 0m, Status = "Unpaid", IsActive = true }
        };
    }
}
