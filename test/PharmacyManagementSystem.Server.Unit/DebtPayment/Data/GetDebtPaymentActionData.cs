using PharmacyManagementSystem.Common.DebtPayment;

namespace PharmacyManagementSystem.Server.Unit.DebtPayment.Data;

public static class GetDebtPaymentActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var debtRecordId = Guid.NewGuid();

        yield return new object[]
        {
            new DebtPaymentFilter { DebtRecordId = debtRecordId },
            new List<Common.DebtPayment.DebtPayment>
            {
                new() { Id = Guid.NewGuid(), DebtRecordId = debtRecordId, AmountPaid = 500m, PaymentMethod = "Cash", ReceivedBy = "pharmacist1", IsActive = true }
            }
        };

        yield return new object[]
        {
            new DebtPaymentFilter(),
            new List<Common.DebtPayment.DebtPayment>
            {
                new() { Id = Guid.NewGuid(), DebtRecordId = debtRecordId, AmountPaid = 500m, PaymentMethod = "Cash", ReceivedBy = "pharmacist1", IsActive = true },
                new() { Id = Guid.NewGuid(), DebtRecordId = Guid.NewGuid(), AmountPaid = 1000m, PaymentMethod = "UPI", ReceivedBy = "pharmacist2", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.DebtPayment.DebtPayment { Id = id, DebtRecordId = Guid.NewGuid(), AmountPaid = 500m, PaymentMethod = "Cash", ReceivedBy = "pharmacist1", IsActive = true }
        };
    }
}
