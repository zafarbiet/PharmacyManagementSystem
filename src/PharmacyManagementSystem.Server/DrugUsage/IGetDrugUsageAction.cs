using PharmacyManagementSystem.Common.DrugUsage;

namespace PharmacyManagementSystem.Server.DrugUsage;

public interface IGetDrugUsageAction
{
    Task<IReadOnlyCollection<Common.DrugUsage.DrugUsage>?> GetByFilterCriteriaAsync(DrugUsageFilter filter, CancellationToken cancellationToken);
    Task<Common.DrugUsage.DrugUsage?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
