namespace PharmacyManagementSystem.Server.Branch;

public interface ISaveBranchAction
{
    Task<Common.Branch.Branch?> AddAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken);
    Task<Common.Branch.Branch?> UpdateAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
