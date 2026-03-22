using PharmacyManagementSystem.Common.DisposalRecord;

namespace PharmacyManagementSystem.Server.DisposalRecord;

public interface IGetDisposalRecordAction
{
    Task<IReadOnlyCollection<Common.DisposalRecord.DisposalRecord>?> GetByFilterCriteriaAsync(DisposalRecordFilter filter, CancellationToken cancellationToken);
    Task<Common.DisposalRecord.DisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
