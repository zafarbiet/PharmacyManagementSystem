using PharmacyManagementSystem.Common.Patient;

namespace PharmacyManagementSystem.Server.Unit.Patient.Data;

public static class GetPatientActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        yield return new object[]
        {
            new PatientFilter { Name = "John Doe" },
            new List<Common.Patient.Patient>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), Name = "John Doe", ContactNumber = "0400000001", Email = "john.doe@example.com" }
            }
        };

        yield return new object[]
        {
            new PatientFilter(),
            new List<Common.Patient.Patient>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), Name = "John Doe", ContactNumber = "0400000001", Email = "john.doe@example.com" },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), Name = "Jane Smith", ContactNumber = "0400000002", Email = "jane.smith@example.com" }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        yield return new object[]
        {
            id.ToString(),
            new Common.Patient.Patient { Id = id, Name = "John Doe", ContactNumber = "0400000001", Email = "john.doe@example.com" }
        };
    }
}
