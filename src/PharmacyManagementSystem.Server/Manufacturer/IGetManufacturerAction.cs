using PharmacyManagementSystem.Common.Manufacturer;

namespace PharmacyManagementSystem.Server.Manufacturer;

public interface IGetManufacturerAction
{
    Task<IReadOnlyCollection<Common.Manufacturer.Manufacturer>?> GetByFilterCriteriaAsync(ManufacturerFilter filter, CancellationToken cancellationToken);
    Task<Common.Manufacturer.Manufacturer?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
