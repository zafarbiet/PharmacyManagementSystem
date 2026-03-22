namespace PharmacyManagementSystem.Server.DamageDisposalRecord;

public interface ISaveDamageDisposalRecordAction
{
    Task<Common.DamageDisposalRecord.DamageDisposalRecord?> AddAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken);
    Task<Common.DamageDisposalRecord.DamageDisposalRecord?> UpdateAsync(Common.DamageDisposalRecord.DamageDisposalRecord? damageDisposalRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
