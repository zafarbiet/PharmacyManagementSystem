using PharmacyManagementSystem.Common.Patient;

namespace PharmacyManagementSystem.Server.Patient;

public interface IGetPatientAction
{
    Task<IReadOnlyCollection<Common.Patient.Patient>?> GetByFilterCriteriaAsync(PatientFilter filter, CancellationToken cancellationToken);
    Task<Common.Patient.Patient?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
