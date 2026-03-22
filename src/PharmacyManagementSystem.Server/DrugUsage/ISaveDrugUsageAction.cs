namespace PharmacyManagementSystem.Server.DrugUsage;

public interface ISaveDrugUsageAction
{
    Task<Common.DrugUsage.DrugUsage?> AddAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken);
    Task<Common.DrugUsage.DrugUsage?> UpdateAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
