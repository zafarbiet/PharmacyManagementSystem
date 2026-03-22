using PharmacyManagementSystem.Common.Vendor;

namespace PharmacyManagementSystem.Server.Vendor;

public interface IGetVendorAction
{
    Task<IReadOnlyCollection<Common.Vendor.Vendor>?> GetByFilterCriteriaAsync(VendorFilter filter, CancellationToken cancellationToken);
    Task<Common.Vendor.Vendor?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
