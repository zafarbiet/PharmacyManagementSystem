namespace PharmacyManagementSystem.Server.Unit.PrescriptionItem.Data;

public static class SavePrescriptionItemActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var prescriptionId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            new Common.PrescriptionItem.PrescriptionItem { PrescriptionId = prescriptionId, DrugId = drugId, Quantity = 30 },
            new Common.PrescriptionItem.PrescriptionItem { Id = new Guid("11111111-1111-1111-1111-111111111111"), PrescriptionId = prescriptionId, DrugId = drugId, Quantity = 30, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        var prescriptionId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        yield return new object[]
        {
            new Common.PrescriptionItem.PrescriptionItem { PrescriptionId = Guid.Empty, DrugId = drugId, Quantity = 30 }
        };

        yield return new object[]
        {
            new Common.PrescriptionItem.PrescriptionItem { PrescriptionId = prescriptionId, DrugId = Guid.Empty, Quantity = 30 }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var prescriptionId = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var drugId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        yield return new object[]
        {
            new Common.PrescriptionItem.PrescriptionItem { Id = id, PrescriptionId = prescriptionId, DrugId = drugId, Quantity = 60 },
            new Common.PrescriptionItem.PrescriptionItem { Id = id, PrescriptionId = prescriptionId, DrugId = drugId, Quantity = 60, UpdatedBy = "system" }
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
