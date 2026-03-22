namespace PharmacyManagementSystem.Server.PrescriptionItem;

public interface ISavePrescriptionItemAction
{
    Task<Common.PrescriptionItem.PrescriptionItem?> AddAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken);
    Task<Common.PrescriptionItem.PrescriptionItem?> UpdateAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
