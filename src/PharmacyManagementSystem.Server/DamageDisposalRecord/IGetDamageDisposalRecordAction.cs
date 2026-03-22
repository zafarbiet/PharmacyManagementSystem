using PharmacyManagementSystem.Common.DamageDisposalRecord;

namespace PharmacyManagementSystem.Server.DamageDisposalRecord;

public interface IGetDamageDisposalRecordAction
{
    Task<IReadOnlyCollection<Common.DamageDisposalRecord.DamageDisposalRecord>?> GetByFilterCriteriaAsync(DamageDisposalRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.DamageDisposalRecord.DamageDisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
