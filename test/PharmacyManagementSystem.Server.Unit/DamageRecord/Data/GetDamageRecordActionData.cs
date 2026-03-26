using PharmacyManagementSystem.Common.DamageRecord;

namespace PharmacyManagementSystem.Server.Unit.DamageRecord.Data;

public static class GetDamageRecordActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var drugInventoryId = Guid.NewGuid();

        yield return new object[]
        {
            new DamageRecordFilter { Status = "Pending" },
            new List<Common.DamageRecord.DamageRecord>
            {
                new() { Id = Guid.NewGuid(), DrugInventoryId = drugInventoryId, QuantityDamaged = 5, DamageType = "Physical", DiscoveredBy = "pharmacist1", Status = "Pending", IsActive = true }
            }
        };

        yield return new object[]
        {
            new DamageRecordFilter(),
            new List<Common.DamageRecord.DamageRecord>
            {
                new() { Id = Guid.NewGuid(), DrugInventoryId = drugInventoryId, QuantityDamaged = 5, DamageType = "Physical", DiscoveredBy = "pharmacist1", Status = "Pending", IsActive = true },
                new() { Id = Guid.NewGuid(), DrugInventoryId = Guid.NewGuid(), QuantityDamaged = 2, DamageType = "Moisture", DiscoveredBy = "pharmacist2", Status = "Disposed", IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.DamageRecord.DamageRecord { Id = id, DrugInventoryId = Guid.NewGuid(), QuantityDamaged = 5, DamageType = "Physical", DiscoveredBy = "pharmacist1", Status = "Pending", IsActive = true }
        };
    }
}
