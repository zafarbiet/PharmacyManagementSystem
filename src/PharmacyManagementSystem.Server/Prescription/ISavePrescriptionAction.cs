namespace PharmacyManagementSystem.Server.Prescription;

public interface ISavePrescriptionAction
{
    Task<Common.Prescription.Prescription?> AddAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken);
    Task<Common.Prescription.Prescription?> UpdateAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
