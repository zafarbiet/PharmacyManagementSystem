using PharmacyManagementSystem.Common.ExpiryRecord;

namespace PharmacyManagementSystem.Server.ExpiryRecord;

public interface IGetExpiryRecordAction
{
    Task<IReadOnlyCollection<Common.ExpiryRecord.ExpiryRecord>?> GetByFilterCriteriaAsync(ExpiryRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.ExpiryRecord.ExpiryRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
