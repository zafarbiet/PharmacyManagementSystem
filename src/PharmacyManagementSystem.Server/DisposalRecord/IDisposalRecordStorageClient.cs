using PharmacyManagementSystem.Common.DisposalRecord;

namespace PharmacyManagementSystem.Server.DisposalRecord;

public interface IDisposalRecordStorageClient
{
    Task<IReadOnlyCollection<Common.DisposalRecord.DisposalRecord>?> GetByFilterCriteriaAsync(DisposalRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.DisposalRecord.DisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DisposalRecord.DisposalRecord?> AddAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken);
    Task<Common.DisposalRecord.DisposalRecord?> UpdateAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
