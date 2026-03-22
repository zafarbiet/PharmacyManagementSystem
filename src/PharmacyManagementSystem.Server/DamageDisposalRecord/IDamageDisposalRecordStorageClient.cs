using PharmacyManagementSystem.Common.DamageDisposalRecord;

namespace PharmacyManagementSystem.Server.DamageDisposalRecord;

public interface IDamageDisposalRecordStorageClient
{
    Task<IReadOnlyCollection<Common.DamageDisposalRecord.DamageDisposalRecord>?> GetByFilterCriteriaAsync(DamageDisposalRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.DamageDisposalRecord.DamageDisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DamageDisposalRecord.DamageDisposalRecord?> AddAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken);
    Task<Common.DamageDisposalRecord.DamageDisposalRecord?> UpdateAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
