namespace PharmacyManagementSystem.Server.Unit.ExpiryRecord.Data;

public static class SaveExpiryRecordActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var drugInventoryId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.ExpiryRecord.ExpiryRecord { DrugInventoryId = drugInventoryId, Status = "Detected", InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5) },
            new Common.ExpiryRecord.ExpiryRecord { Id = Guid.NewGuid(), DrugInventoryId = drugInventoryId, Status = "Detected", InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5), IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.ExpiryRecord.ExpiryRecord { DrugInventoryId = Guid.Empty, Status = "Detected", InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5) }
        };

        yield return new object[]
        {
            new Common.ExpiryRecord.ExpiryRecord { DrugInventoryId = Guid.NewGuid(), Status = string.Empty, InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5) }
        };

        yield return new object[]
        {
            new Common.ExpiryRecord.ExpiryRecord { DrugInventoryId = Guid.NewGuid(), Status = "Detected", InitiatedBy = string.Empty, QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5) }
        };

        yield return new object[]
        {
            new Common.ExpiryRecord.ExpiryRecord { DrugInventoryId = Guid.NewGuid(), Status = "Detected", InitiatedBy = "admin", QuantityAffected = 0, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5) }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var drugInventoryId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.ExpiryRecord.ExpiryRecord { Id = id, DrugInventoryId = drugInventoryId, Status = "Approved", InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5) },
            new Common.ExpiryRecord.ExpiryRecord { Id = id, DrugInventoryId = drugInventoryId, Status = "Approved", InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5), IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> ValidRemoveData()
    {
        yield return new object[]
        {
            Guid.NewGuid(),
            "system"
        };
    }
}
