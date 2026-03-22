using PharmacyManagementSystem.Common.DisposalRecord;

namespace PharmacyManagementSystem.Server.Unit.DisposalRecord.Data;

public static class GetDisposalRecordActionData
{
    public static IEnumerable<object[]> ValidFilterData()
    {
        var expiryRecordId = Guid.NewGuid();
        yield return new object[]
        {
            new DisposalRecordFilter { ExpiryRecordId = expiryRecordId },
            new List<Common.DisposalRecord.DisposalRecord>
            {
                new() { Id = Guid.NewGuid(), ExpiryRecordId = expiryRecordId, DisposalMethod = "Incineration", DisposedBy = "admin", QuantityDisposed = 10, DisposedAt = DateTimeOffset.UtcNow, IsActive = true }
            }
        };

        yield return new object[]
        {
            new DisposalRecordFilter(),
            new List<Common.DisposalRecord.DisposalRecord>
            {
                new() { Id = Guid.NewGuid(), ExpiryRecordId = expiryRecordId, DisposalMethod = "Incineration", DisposedBy = "admin", QuantityDisposed = 10, DisposedAt = DateTimeOffset.UtcNow, IsActive = true },
                new() { Id = Guid.NewGuid(), ExpiryRecordId = Guid.NewGuid(), DisposalMethod = "Chemical Neutralization", DisposedBy = "pharmacist", QuantityDisposed = 5, DisposedAt = DateTimeOffset.UtcNow, IsActive = true }
            }
        };
    }

    public static IEnumerable<object[]> ValidIdData()
    {
        var id = Guid.NewGuid();
        yield return new object[]
        {
            id.ToString(),
            new Common.DisposalRecord.DisposalRecord { Id = id, ExpiryRecordId = Guid.NewGuid(), DisposalMethod = "Incineration", DisposedBy = "admin", QuantityDisposed = 10, DisposedAt = DateTimeOffset.UtcNow, IsActive = true }
        };
    }
}
