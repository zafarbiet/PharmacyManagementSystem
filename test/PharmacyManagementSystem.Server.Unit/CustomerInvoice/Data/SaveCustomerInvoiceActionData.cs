namespace PharmacyManagementSystem.Server.Unit.CustomerInvoice.Data;

public static class SaveCustomerInvoiceActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var patientId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.CustomerInvoice.CustomerInvoice { PatientId = patientId, Status = "Pending", NetAmount = 150.00m },
            new Common.CustomerInvoice.CustomerInvoice { Id = new Guid("11111111-1111-1111-1111-111111111111"), PatientId = patientId, Status = "Pending", NetAmount = 150.00m, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.CustomerInvoice.CustomerInvoice { Status = string.Empty, NetAmount = 150.00m }
        };

        yield return new object[]
        {
            new Common.CustomerInvoice.CustomerInvoice { Status = "   ", NetAmount = 150.00m }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var patientId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.CustomerInvoice.CustomerInvoice { Id = id, PatientId = patientId, Status = "Paid", NetAmount = 150.00m },
            new Common.CustomerInvoice.CustomerInvoice { Id = id, PatientId = patientId, Status = "Paid", NetAmount = 150.00m, UpdatedBy = "system" }
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
