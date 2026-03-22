namespace PharmacyManagementSystem.Server.Unit.Prescription.Data;

public static class SavePrescriptionActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var patientId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.Prescription.Prescription { PatientId = patientId, PrescribingDoctor = "Dr. Smith" },
            new Common.Prescription.Prescription { Id = new Guid("11111111-1111-1111-1111-111111111111"), PatientId = patientId, PrescribingDoctor = "Dr. Smith", UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.Prescription.Prescription { PatientId = Guid.Empty, PrescribingDoctor = "Dr. Smith" }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var patientId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.Prescription.Prescription { Id = id, PatientId = patientId, PrescribingDoctor = "Dr. Updated" },
            new Common.Prescription.Prescription { Id = id, PatientId = patientId, PrescribingDoctor = "Dr. Updated", UpdatedBy = "system" }
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
