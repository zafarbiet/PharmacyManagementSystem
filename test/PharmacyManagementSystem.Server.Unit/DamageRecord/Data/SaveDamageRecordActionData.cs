namespace PharmacyManagementSystem.Server.Unit.DamageRecord.Data;

public static class SaveDamageRecordActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var drugInventoryId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DamageRecord.DamageRecord { DrugInventoryId = drugInventoryId, QuantityDamaged = 5, DamageType = "Physical", DiscoveredBy = "pharmacist1", Status = "Pending" },
            new Common.DamageRecord.DamageRecord { Id = Guid.NewGuid(), DrugInventoryId = drugInventoryId, QuantityDamaged = 5, DamageType = "Physical", DiscoveredBy = "pharmacist1", Status = "Pending", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DamageRecord.DamageRecord { DrugInventoryId = Guid.Empty, QuantityDamaged = 5, DamageType = "Physical", DiscoveredBy = "admin", Status = "Pending" }
        };

        yield return new object[]
        {
            new Common.DamageRecord.DamageRecord { DrugInventoryId = Guid.NewGuid(), QuantityDamaged = 0, DamageType = "Physical", DiscoveredBy = "admin", Status = "Pending" }
        };

        yield return new object[]
        {
            new Common.DamageRecord.DamageRecord { DrugInventoryId = Guid.NewGuid(), QuantityDamaged = 5, DamageType = string.Empty, DiscoveredBy = "admin", Status = "Pending" }
        };

        yield return new object[]
        {
            new Common.DamageRecord.DamageRecord { DrugInventoryId = Guid.NewGuid(), QuantityDamaged = 5, DamageType = "Physical", DiscoveredBy = string.Empty, Status = "Pending" }
        };

        yield return new object[]
        {
            new Common.DamageRecord.DamageRecord { DrugInventoryId = Guid.NewGuid(), QuantityDamaged = 5, DamageType = "Physical", DiscoveredBy = "admin", Status = string.Empty }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var drugInventoryId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DamageRecord.DamageRecord { Id = id, DrugInventoryId = drugInventoryId, QuantityDamaged = 3, DamageType = "Moisture", DiscoveredBy = "admin", Status = "Approved" },
            new Common.DamageRecord.DamageRecord { Id = id, DrugInventoryId = drugInventoryId, QuantityDamaged = 3, DamageType = "Moisture", DiscoveredBy = "admin", Status = "Approved", IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[] { Guid.NewGuid(), "system" };
    }
}
