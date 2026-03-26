using PharmacyManagementSystem.Common.Manufacturer;

namespace PharmacyManagementSystem.Server.Manufacturer;

public interface IManufacturerRepository
{
    Task<IReadOnlyCollection<Common.Manufacturer.Manufacturer>?> GetByFilterCriteriaAsync(ManufacturerFilter filter, CancellationToken cancellationToken);
    Task<Common.Manufacturer.Manufacturer?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Manufacturer.Manufacturer?> AddAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken);
    Task<Common.Manufacturer.Manufacturer?> UpdateAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
