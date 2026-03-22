namespace PharmacyManagementSystem.Server.Drug;

public interface ISaveDrugAction
{
    Task<Common.Drug.Drug?> AddAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken);
    Task<Common.Drug.Drug?> UpdateAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
