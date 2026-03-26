namespace PharmacyManagementSystem.Server.Unit.PaymentLedger.Data;

public static class SavePaymentLedgerActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.PaymentLedger.PaymentLedger { VendorId = vendorId, InvoicedAmount = 50000m },
            new Common.PaymentLedger.PaymentLedger { Id = Guid.NewGuid(), VendorId = vendorId, InvoicedAmount = 50000m, PaidAmount = 0m, Status = "Unpaid", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.PaymentLedger.PaymentLedger { VendorId = Guid.Empty, InvoicedAmount = 50000m }
        };

        yield return new object[]
        {
            new Common.PaymentLedger.PaymentLedger { VendorId = Guid.NewGuid(), InvoicedAmount = 0m }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.PaymentLedger.PaymentLedger { Id = id, VendorId = vendorId, InvoicedAmount = 50000m, PaidAmount = 25000m, Status = "PartiallyPaid" },
            new Common.PaymentLedger.PaymentLedger { Id = id, VendorId = vendorId, InvoicedAmount = 50000m, PaidAmount = 25000m, Status = "PartiallyPaid", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }

    public static IEnumerable<object[]> RecordPaymentData()
    {
        var ledgerId = Guid.NewGuid();
        var vendorId = Guid.NewGuid();
        yield return new object[]
        {
            ledgerId,
            25000m,
            new Common.PaymentLedger.PaymentLedger { Id = ledgerId, VendorId = vendorId, InvoicedAmount = 50000m, PaidAmount = 0m, Status = "Unpaid", IsActive = true },
            new Common.Vendor.Vendor { Id = vendorId, Name = "MedSupply Co", OutstandingBalance = 50000m, IsActive = true }
        };
    }
}
