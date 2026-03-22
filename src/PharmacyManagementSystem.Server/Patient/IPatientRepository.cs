using PharmacyManagementSystem.Common.Patient;

namespace PharmacyManagementSystem.Server.Patient;

public interface IPatientRepository
{
    Task<IReadOnlyCollection<Common.Patient.Patient>?> GetByFilterCriteriaAsync(PatientFilter filter, CancellationToken cancellationToken);
    Task<Common.Patient.Patient?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Patient.Patient?> AddAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken);
    Task<Common.Patient.Patient?> UpdateAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
