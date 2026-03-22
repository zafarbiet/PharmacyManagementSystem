using PharmacyManagementSystem.Common.ExpiryRecord;

namespace PharmacyManagementSystem.Server.Unit.ExpiryRecord.Data;

public static class GetExpiryRecordActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var drugInventoryId = Guid.NewGuid();
        yield return new object[]
        {
            new ExpiryRecordFilter { DrugInventoryId = drugInventoryId },
            new List<Common.ExpiryRecord.ExpiryRecord>
            {
                new() { Id = Guid.NewGuid(), DrugInventoryId = drugInventoryId, Status = "Detected", InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5), IsActive = true }
            }
        };

        yield return new object[]
        {
            new ExpiryRecordFilter(),
            new List<Common.ExpiryRecord.ExpiryRecord>
            {
                new() { Id = Guid.NewGuid(), DrugInventoryId = drugInventoryId, Status = "Detected", InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5), IsActive = true },
                new() { Id = Guid.NewGuid(), DrugInventoryId = Guid.NewGuid(), Status = "Disposed", InitiatedBy = "admin", QuantityAffected = 5, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-10), IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.ExpiryRecord.ExpiryRecord { Id = id, DrugInventoryId = Guid.NewGuid(), Status = "Detected", InitiatedBy = "admin", QuantityAffected = 10, DetectedAt = DateTimeOffset.UtcNow, ExpirationDate = DateTimeOffset.UtcNow.AddDays(-5), IsActive = true }
        };
    }
}
