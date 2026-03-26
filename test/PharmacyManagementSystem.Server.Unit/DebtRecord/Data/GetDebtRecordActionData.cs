using PharmacyManagementSystem.Common.DebtRecord;

namespace PharmacyManagementSystem.Server.Unit.DebtRecord.Data;

public static class GetDebtRecordActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var patientId = Guid.NewGuid();

        yield return new object[]
        {
            new DebtRecordFilter { PatientId = patientId },
            new List<Common.DebtRecord.DebtRecord>
            {
                new() { Id = Guid.NewGuid(), PatientId = patientId, InvoiceId = Guid.NewGuid(), OriginalAmount = 1500m, RemainingAmount = 1500m, Status = "Unpaid", IsActive = true }
            }
        };

        yield return new object[]
        {
            new DebtRecordFilter(),
            new List<Common.DebtRecord.DebtRecord>
            {
                new() { Id = Guid.NewGuid(), PatientId = patientId, InvoiceId = Guid.NewGuid(), OriginalAmount = 1500m, RemainingAmount = 1500m, Status = "Unpaid", IsActive = true },
                new() { Id = Guid.NewGuid(), PatientId = Guid.NewGuid(), InvoiceId = Guid.NewGuid(), OriginalAmount = 500m, RemainingAmount = 0m, Status = "Paid", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.DebtRecord.DebtRecord { Id = id, PatientId = Guid.NewGuid(), InvoiceId = Guid.NewGuid(), OriginalAmount = 1500m, RemainingAmount = 1500m, Status = "Unpaid", IsActive = true }
        };
    }
}
