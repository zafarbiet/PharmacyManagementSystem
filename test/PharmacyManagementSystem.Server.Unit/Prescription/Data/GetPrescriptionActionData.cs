using PharmacyManagementSystem.Common.Prescription;

namespace PharmacyManagementSystem.Server.Unit.Prescription.Data;

public static class GetPrescriptionActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var patientId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new PrescriptionFilter { PatientId = patientId },
            new List<Common.Prescription.Prescription>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), PatientId = patientId, PrescribingDoctor = "Dr. Smith" }
            }
        };

        yield return new object[]
        {
            new PrescriptionFilter(),
            new List<Common.Prescription.Prescription>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), PatientId = patientId, PrescribingDoctor = "Dr. Smith" },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), PatientId = patientId, PrescribingDoctor = "Dr. Jones" }
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
            new Common.Prescription.Prescription { Id = id, PatientId = patientId, PrescribingDoctor = "Dr. Smith" }
        };
    }
}
