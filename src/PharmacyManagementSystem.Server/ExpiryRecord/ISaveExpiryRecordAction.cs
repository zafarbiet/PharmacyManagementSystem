namespace PharmacyManagementSystem.Server.ExpiryRecord;

public interface ISaveExpiryRecordAction
{
    Task<Common.ExpiryRecord.ExpiryRecord?> AddAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken);
    Task<Common.ExpiryRecord.ExpiryRecord?> UpdateAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
