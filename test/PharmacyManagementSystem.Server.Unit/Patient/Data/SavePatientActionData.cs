namespace PharmacyManagementSystem.Server.Unit.Patient.Data;

public static class SavePatientActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        yield return new object[]
        {
            new Common.Patient.Patient { Name = "John Doe", ContactNumber = "0400000001", Email = "john.doe@example.com" },
            new Common.Patient.Patient { Id = new Guid("11111111-1111-1111-1111-111111111111"), Name = "John Doe", ContactNumber = "0400000001", Email = "john.doe@example.com", UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.Patient.Patient { Name = string.Empty, ContactNumber = "0400000001" }
        };

        yield return new object[]
        {
            new Common.Patient.Patient { Name = "   ", ContactNumber = "0400000001" }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        yield return new object[]
        {
            new Common.Patient.Patient { Id = id, Name = "John Doe Updated", ContactNumber = "0400000001" },
            new Common.Patient.Patient { Id = id, Name = "John Doe Updated", ContactNumber = "0400000001", UpdatedBy = "system" }
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
