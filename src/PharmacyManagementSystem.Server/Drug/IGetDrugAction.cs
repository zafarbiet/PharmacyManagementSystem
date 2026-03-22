using PharmacyManagementSystem.Common.Drug;

namespace PharmacyManagementSystem.Server.Drug;

public interface IGetDrugAction
{
    Task<IReadOnlyCollection<Common.Drug.Drug>?> GetByFilterCriteriaAsync(DrugFilter filter, CancellationToken cancellationToken);
    Task<Common.Drug.Drug?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
