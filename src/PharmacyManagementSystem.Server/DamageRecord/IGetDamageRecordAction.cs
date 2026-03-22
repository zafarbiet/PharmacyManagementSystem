using PharmacyManagementSystem.Common.DamageRecord;
namespace PharmacyManagementSystem.Server.DamageRecord;
public interface IGetDamageRecordAction
{
    Task<IReadOnlyCollection<Common.DamageRecord.DamageRecord>?> GetByFilterCriteriaAsync(DamageRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.DamageRecord.DamageRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
