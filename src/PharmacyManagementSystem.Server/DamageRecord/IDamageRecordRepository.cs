using PharmacyManagementSystem.Common.DamageRecord;
namespace PharmacyManagementSystem.Server.DamageRecord;
public interface IDamageRecordRepository
{
    Task<IReadOnlyCollection<Common.DamageRecord.DamageRecord>?> GetByFilterCriteriaAsync(DamageRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.DamageRecord.DamageRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DamageRecord.DamageRecord?> AddAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken);
    Task<Common.DamageRecord.DamageRecord?> UpdateAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
