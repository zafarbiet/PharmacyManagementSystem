namespace PharmacyManagementSystem.Server.Unit.DisposalRecord.Data;

public static class SaveDisposalRecordActionData
{
    public static IEnumerable<object[]> ValidAddData()
    {
        var expiryRecordId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DisposalRecord.DisposalRecord { ExpiryRecordId = expiryRecordId, DisposalMethod = "Incineration", DisposedBy = "admin", QuantityDisposed = 10, DisposedAt = DateTimeOffset.UtcNow },
            new Common.DisposalRecord.DisposalRecord { Id = Guid.NewGuid(), ExpiryRecordId = expiryRecordId, DisposalMethod = "Incineration", DisposedBy = "admin", QuantityDisposed = 10, DisposedAt = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
        };
    }

    public static IEnumerable<object[]> InvalidAddData()
    {
        yield return new object[]
        {
            new Common.DisposalRecord.DisposalRecord { ExpiryRecordId = Guid.Empty, DisposalMethod = "Incineration", DisposedBy = "admin", QuantityDisposed = 10, DisposedAt = DateTimeOffset.UtcNow }
        };

        yield return new object[]
        {
            new Common.DisposalRecord.DisposalRecord { ExpiryRecordId = Guid.NewGuid(), DisposalMethod = string.Empty, DisposedBy = "admin", QuantityDisposed = 10, DisposedAt = DateTimeOffset.UtcNow }
        };

        yield return new object[]
        {
            new Common.DisposalRecord.DisposalRecord { ExpiryRecordId = Guid.NewGuid(), DisposalMethod = "Incineration", DisposedBy = string.Empty, QuantityDisposed = 10, DisposedAt = DateTimeOffset.UtcNow }
        };

        yield return new object[]
        {
            new Common.DisposalRecord.DisposalRecord { ExpiryRecordId = Guid.NewGuid(), DisposalMethod = "Incineration", DisposedBy = "admin", QuantityDisposed = 0, DisposedAt = DateTimeOffset.UtcNow }
        };
    }

    public static IEnumerable<object[]> ValidUpdateData()
    {
        var id = Guid.NewGuid();
        var expiryRecordId = Guid.NewGuid();
        yield return new object[]
        {
            new Common.DisposalRecord.DisposalRecord { Id = id, ExpiryRecordId = expiryRecordId, DisposalMethod = "Chemical Neutralization", DisposedBy = "pharmacist", QuantityDisposed = 5, DisposedAt = DateTimeOffset.UtcNow },
            new Common.DisposalRecord.DisposalRecord { Id = id, ExpiryRecordId = expiryRecordId, DisposalMethod = "Chemical Neutralization", DisposedBy = "pharmacist", QuantityDisposed = 5, DisposedAt = DateTimeOffset.UtcNow, IsActive = true, UpdatedBy = "system" }
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
