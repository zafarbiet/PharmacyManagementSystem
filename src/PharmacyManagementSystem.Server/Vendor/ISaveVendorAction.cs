namespace PharmacyManagementSystem.Server.Vendor;

public interface ISaveVendorAction
{
    Task<Common.Vendor.Vendor?> AddAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken);
    Task<Common.Vendor.Vendor?> UpdateAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
