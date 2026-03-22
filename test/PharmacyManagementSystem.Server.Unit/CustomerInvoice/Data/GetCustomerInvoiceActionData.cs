using PharmacyManagementSystem.Common.CustomerInvoice;

namespace PharmacyManagementSystem.Server.Unit.CustomerInvoice.Data;

public static class GetCustomerInvoiceActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var patientId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new CustomerInvoiceFilter { PatientId = patientId },
            new List<Common.CustomerInvoice.CustomerInvoice>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), PatientId = patientId, Status = "Paid", NetAmount = 150.00m }
            }
        };

        yield return new object[]
        {
            new CustomerInvoiceFilter(),
            new List<Common.CustomerInvoice.CustomerInvoice>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), PatientId = patientId, Status = "Paid", NetAmount = 150.00m },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), PatientId = patientId, Status = "Pending", NetAmount = 75.00m }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var patientId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            id.ToString(),
            new Common.CustomerInvoice.CustomerInvoice { Id = id, PatientId = patientId, Status = "Paid", NetAmount = 150.00m }
        };
    }
}
