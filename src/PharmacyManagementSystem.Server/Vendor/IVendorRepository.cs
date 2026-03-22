using PharmacyManagementSystem.Common.Vendor;

namespace PharmacyManagementSystem.Server.Vendor;

public interface IVendorRepository
{
    Task<IReadOnlyCollection<Common.Vendor.Vendor>?> GetByFilterCriteriaAsync(VendorFilter filter, CancellationToken cancellationToken);
    Task<Common.Vendor.Vendor?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.Vendor.Vendor?> AddAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken);
    Task<Common.Vendor.Vendor?> UpdateAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
