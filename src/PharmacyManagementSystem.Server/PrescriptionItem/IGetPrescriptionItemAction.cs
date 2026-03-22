using PharmacyManagementSystem.Common.PrescriptionItem;

namespace PharmacyManagementSystem.Server.PrescriptionItem;

public interface IGetPrescriptionItemAction
{
    Task<IReadOnlyCollection<Common.PrescriptionItem.PrescriptionItem>?> GetByFilterCriteriaAsync(PrescriptionItemFilter filter, CancellationToken cancellationToken);
    Task<Common.PrescriptionItem.PrescriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
