using PharmacyManagementSystem.Common.DamageDisposalRecord;

namespace PharmacyManagementSystem.Server.Unit.DamageDisposalRecord.Data;

public static class GetDamageDisposalRecordActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var damageRecordId = Guid.NewGuid();

        yield return new object[]
        {
            new DamageDisposalRecordFilter { DamageRecordId = damageRecordId },
            new List<Common.DamageDisposalRecord.DamageDisposalRecord>
            {
                new() { Id = Guid.NewGuid(), DamageRecordId = damageRecordId, DisposalMethod = "Incineration", DisposedBy = "pharmacist1", IsActive = true }
            }
        };

        yield return new object[]
        {
            new DamageDisposalRecordFilter(),
            new List<Common.DamageDisposalRecord.DamageDisposalRecord>
            {
                new() { Id = Guid.NewGuid(), DamageRecordId = damageRecordId, DisposalMethod = "Incineration", DisposedBy = "pharmacist1", IsActive = true },
                new() { Id = Guid.NewGuid(), DamageRecordId = Guid.NewGuid(), DisposalMethod = "Landfill", DisposedBy = "pharmacist2", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.DamageDisposalRecord.DamageDisposalRecord { Id = id, DamageRecordId = Guid.NewGuid(), DisposalMethod = "Incineration", DisposedBy = "pharmacist1", IsActive = true }
        };
    }
}
