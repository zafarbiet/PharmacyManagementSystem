namespace PharmacyManagementSystem.Server.Patient;

public interface ISavePatientAction
{
    Task<Common.Patient.Patient?> AddAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken);
    Task<Common.Patient.Patient?> UpdateAsync(Common.Patient.Patient? patient, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
