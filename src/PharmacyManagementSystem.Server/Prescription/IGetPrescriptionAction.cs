using PharmacyManagementSystem.Common.Prescription;

namespace PharmacyManagementSystem.Server.Prescription;

public interface IGetPrescriptionAction
{
    Task<IReadOnlyCollection<Common.Prescription.Prescription>?> GetByFilterCriteriaAsync(PrescriptionFilter filter, CancellationToken cancellationToken);
    Task<Common.Prescription.Prescription?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
