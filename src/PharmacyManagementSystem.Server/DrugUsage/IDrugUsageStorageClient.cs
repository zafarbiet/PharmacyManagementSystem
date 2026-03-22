using PharmacyManagementSystem.Common.DrugUsage;

namespace PharmacyManagementSystem.Server.DrugUsage;

public interface IDrugUsageStorageClient
{
    Task<IReadOnlyCollection<Common.DrugUsage.DrugUsage>?> GetByFilterCriteriaAsync(DrugUsageFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugUsage.DrugUsage?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DrugUsage.DrugUsage?> AddAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken);
    Task<Common.DrugUsage.DrugUsage?> UpdateAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
