using PharmacyManagementSystem.Common.PrescriptionItem;

namespace PharmacyManagementSystem.Server.Unit.PrescriptionItem.Data;

public static class GetPrescriptionItemActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var prescriptionId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            new PrescriptionItemFilter { PrescriptionId = prescriptionId },
            new List<Common.PrescriptionItem.PrescriptionItem>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), PrescriptionId = prescriptionId, DrugId = drugId, Quantity = 30 }
            }
        };

        yield return new object[]
        {
            new PrescriptionItemFilter(),
            new List<Common.PrescriptionItem.PrescriptionItem>
            {
                new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), PrescriptionId = prescriptionId, DrugId = drugId, Quantity = 30 },
                new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), PrescriptionId = prescriptionId, DrugId = drugId, Quantity = 60 }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var prescriptionId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            id.ToString(),
            new Common.PrescriptionItem.PrescriptionItem { Id = id, PrescriptionId = prescriptionId, DrugId = drugId, Quantity = 30 }
        };
    }
}
