namespace PharmacyManagementSystem.Server.Manufacturer;

public interface ISaveManufacturerAction
{
    Task<Common.Manufacturer.Manufacturer?> AddAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken);
    Task<Common.Manufacturer.Manufacturer?> UpdateAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
