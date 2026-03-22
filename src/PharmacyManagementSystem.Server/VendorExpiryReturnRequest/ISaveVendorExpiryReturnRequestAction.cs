namespace PharmacyManagementSystem.Server.VendorExpiryReturnRequest;

public interface ISaveVendorExpiryReturnRequestAction
{
    Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> AddAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken);
    Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> UpdateAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
