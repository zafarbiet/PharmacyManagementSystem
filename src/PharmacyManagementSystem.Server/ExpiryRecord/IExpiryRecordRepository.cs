using PharmacyManagementSystem.Common.ExpiryRecord;

namespace PharmacyManagementSystem.Server.ExpiryRecord;

public interface IExpiryRecordRepository
{
    Task<IReadOnlyCollection<Common.ExpiryRecord.ExpiryRecord>?> GetByFilterCriteriaAsync(ExpiryRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.ExpiryRecord.ExpiryRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.ExpiryRecord.ExpiryRecord?> AddAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken);
    Task<Common.ExpiryRecord.ExpiryRecord?> UpdateAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
