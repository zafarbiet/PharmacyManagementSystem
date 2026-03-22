using PharmacyManagementSystem.Common.DebtRecord;

namespace PharmacyManagementSystem.Server.DebtRecord;

public interface IGetDebtRecordAction
{
    Task<IReadOnlyCollection<Common.DebtRecord.DebtRecord>?> GetByFilterCriteriaAsync(DebtRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.DebtRecord.DebtRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
