namespace PharmacyManagementSystem.Server.DamageRecord;
public interface ISaveDamageRecordAction
{
    Task<Common.DamageRecord.DamageRecord?> AddAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken);
    Task<Common.DamageRecord.DamageRecord?> UpdateAsync(Common.DamageRecord.DamageRecord? damageRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
