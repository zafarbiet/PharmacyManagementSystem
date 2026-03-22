namespace PharmacyManagementSystem.Server.DisposalRecord;

public interface ISaveDisposalRecordAction
{
    Task<Common.DisposalRecord.DisposalRecord?> AddAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken);
    Task<Common.DisposalRecord.DisposalRecord?> UpdateAsync(Common.DisposalRecord.DisposalRecord? disposalRecord, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
