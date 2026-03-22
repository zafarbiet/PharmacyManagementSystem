namespace PharmacyManagementSystem.Server.DebtRecord;

public interface ISaveDebtRecordAction
{
    Task<Common.DebtRecord.DebtRecord?> AddAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken);
    Task<Common.DebtRecord.DebtRecord?> UpdateAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
