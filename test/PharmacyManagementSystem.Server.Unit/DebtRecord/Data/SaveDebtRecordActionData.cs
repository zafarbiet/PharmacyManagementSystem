namespace PharmacyManagementSystem.Server.Unit.DebtRecord.Data;

public static class SaveDebtRecordActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var patientId = Guid.NewGuid();
        var invoiceId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DebtRecord.DebtRecord { PatientId = patientId, InvoiceId = invoiceId, OriginalAmount = 1500m, RemainingAmount = 1500m, Status = "Unpaid" },
            new Common.DebtRecord.DebtRecord { Id = Guid.NewGuid(), PatientId = patientId, InvoiceId = invoiceId, OriginalAmount = 1500m, RemainingAmount = 1500m, Status = "Unpaid", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DebtRecord.DebtRecord { PatientId = Guid.Empty, InvoiceId = Guid.NewGuid(), OriginalAmount = 1500m, Status = "Unpaid" }
        };

        yield return new object[]
        {
            new Common.DebtRecord.DebtRecord { PatientId = Guid.NewGuid(), InvoiceId = Guid.Empty, OriginalAmount = 1500m, Status = "Unpaid" }
        };

        yield return new object[]
        {
            new Common.DebtRecord.DebtRecord { PatientId = Guid.NewGuid(), InvoiceId = Guid.NewGuid(), OriginalAmount = 0m, Status = "Unpaid" }
        };

        yield return new object[]
        {
            new Common.DebtRecord.DebtRecord { PatientId = Guid.NewGuid(), InvoiceId = Guid.NewGuid(), OriginalAmount = 1500m, Status = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var invoiceId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DebtRecord.DebtRecord { Id = id, PatientId = patientId, InvoiceId = invoiceId, OriginalAmount = 1500m, RemainingAmount = 500m, Status = "PartiallyPaid" },
            new Common.DebtRecord.DebtRecord { Id = id, PatientId = patientId, InvoiceId = invoiceId, OriginalAmount = 1500m, RemainingAmount = 500m, Status = "PartiallyPaid", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
