using PharmacyManagementSystem.Common.Branch;

namespace PharmacyManagementSystem.Server.Branch;

public interface IGetBranchAction
{
    Task<IReadOnlyCollection<Common.Branch.Branch>?> GetByFilterCriteriaAsync(BranchFilter filter, CancellationToken cancellationToken);
    Task<Common.Branch.Branch?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
