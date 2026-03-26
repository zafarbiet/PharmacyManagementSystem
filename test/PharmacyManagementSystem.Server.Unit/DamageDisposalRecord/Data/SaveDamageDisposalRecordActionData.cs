namespace PharmacyManagementSystem.Server.Unit.DamageDisposalRecord.Data;

public static class SaveDamageDisposalRecordActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var damageRecordId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DamageDisposalRecord.DamageDisposalRecord { DamageRecordId = damageRecordId, DisposalMethod = "Incineration", DisposedBy = "pharmacist1" },
            new Common.DamageDisposalRecord.DamageDisposalRecord { Id = Guid.NewGuid(), DamageRecordId = damageRecordId, DisposalMethod = "Incineration", DisposedBy = "pharmacist1", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DamageDisposalRecord.DamageDisposalRecord { DamageRecordId = Guid.Empty, DisposalMethod = "Incineration", DisposedBy = "admin" }
        };

        yield return new object[]
        {
            new Common.DamageDisposalRecord.DamageDisposalRecord { DamageRecordId = Guid.NewGuid(), DisposalMethod = string.Empty, DisposedBy = "admin" }
        };

        yield return new object[]
        {
            new Common.DamageDisposalRecord.DamageDisposalRecord { DamageRecordId = Guid.NewGuid(), DisposalMethod = "Incineration", DisposedBy = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var damageRecordId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DamageDisposalRecord.DamageDisposalRecord { Id = id, DamageRecordId = damageRecordId, DisposalMethod = "Landfill", DisposedBy = "admin" },
            new Common.DamageDisposalRecord.DamageDisposalRecord { Id = id, DamageRecordId = damageRecordId, DisposalMethod = "Landfill", DisposedBy = "admin", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
