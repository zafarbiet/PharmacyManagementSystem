namespace PharmacyManagementSystem.Server.Unit.DebtPayment.Data;

public static class SaveDebtPaymentActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var debtRecordId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DebtPayment.DebtPayment { DebtRecordId = debtRecordId, AmountPaid = 500m, PaymentMethod = "Cash", ReceivedBy = "pharmacist1" },
            new Common.DebtPayment.DebtPayment { Id = Guid.NewGuid(), DebtRecordId = debtRecordId, AmountPaid = 500m, PaymentMethod = "Cash", ReceivedBy = "pharmacist1", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DebtPayment.DebtPayment { DebtRecordId = Guid.Empty, AmountPaid = 500m, ReceivedBy = "admin" }
        };

        yield return new object[]
        {
            new Common.DebtPayment.DebtPayment { DebtRecordId = Guid.NewGuid(), AmountPaid = 0m, ReceivedBy = "admin" }
        };

        yield return new object[]
        {
            new Common.DebtPayment.DebtPayment { DebtRecordId = Guid.NewGuid(), AmountPaid = 500m, ReceivedBy = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var debtRecordId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DebtPayment.DebtPayment { Id = id, DebtRecordId = debtRecordId, AmountPaid = 750m, PaymentMethod = "UPI", ReceivedBy = "admin" },
            new Common.DebtPayment.DebtPayment { Id = id, DebtRecordId = debtRecordId, AmountPaid = 750m, PaymentMethod = "UPI", ReceivedBy = "admin", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
