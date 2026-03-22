using PharmacyManagementSystem.Common.Drug;

namespace PharmacyManagementSystem.Server.Drug;

public interface IDrugRepository
{
    Task<IReadOnlyCollection<Common.Drug.Drug>?> GetByFilterCriteriaAsync(DrugFilter filter, CancellationToken cancellationToken);
    Task<Common.Drug.Drug?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Drug.Drug?> AddAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken);
    Task<Common.Drug.Drug?> UpdateAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
