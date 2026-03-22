using PharmacyManagementSystem.Common.PrescriptionItem;

namespace PharmacyManagementSystem.Server.PrescriptionItem;

public interface IPrescriptionItemRepository
{
    Task<IReadOnlyCollection<Common.PrescriptionItem.PrescriptionItem>?> GetByFilterCriteriaAsync(PrescriptionItemFilter filter, CancellationToken cancellationToken);
    Task<Common.PrescriptionItem.PrescriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.PrescriptionItem.PrescriptionItem?> AddAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken);
    Task<Common.PrescriptionItem.PrescriptionItem?> UpdateAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
