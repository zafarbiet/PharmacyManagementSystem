using PharmacyManagementSystem.Common.Branch;

namespace PharmacyManagementSystem.Server.Branch;

public interface IBranchStorageClient
{
    Task<IReadOnlyCollection<Common.Branch.Branch>?> GetByFilterCriteriaAsync(BranchFilter filter, CancellationToken cancellationToken);
    Task<Common.Branch.Branch?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Branch.Branch?> AddAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken);
    Task<Common.Branch.Branch?> UpdateAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
