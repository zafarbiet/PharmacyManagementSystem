using PharmacyManagementSystem.Common.Prescription;

namespace PharmacyManagementSystem.Server.Prescription;

public interface IPrescriptionRepository
{
    Task<IReadOnlyCollection<Common.Prescription.Prescription>?> GetByFilterCriteriaAsync(PrescriptionFilter filter, CancellationToken cancellationToken);
    Task<Common.Prescription.Prescription?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Prescription.Prescription?> AddAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken);
    Task<Common.Prescription.Prescription?> UpdateAsync(Common.Prescription.Prescription? prescription, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
