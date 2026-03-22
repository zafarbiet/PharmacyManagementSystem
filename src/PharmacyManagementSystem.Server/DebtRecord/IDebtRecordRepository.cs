using PharmacyManagementSystem.Common.DebtRecord;

namespace PharmacyManagementSystem.Server.DebtRecord;

public interface IDebtRecordRepository
{
    Task<IReadOnlyCollection<Common.DebtRecord.DebtRecord>?> GetByFilterCriteriaAsync(DebtRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.DebtRecord.DebtRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DebtRecord.DebtRecord?> AddAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken);
    Task<Common.DebtRecord.DebtRecord?> UpdateAsync(Common.DebtRecord.DebtRecord? debtRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
