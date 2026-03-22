using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;

namespace PharmacyManagementSystem.Server.VendorExpiryReturnRequest;

public interface IVendorExpiryReturnRequestStorageClient
{
    Task<IReadOnlyCollection<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>?> GetByFilterCriteriaAsync(VendorExpiryReturnRequestFilter filter, CancellationToken cancellationToken);
    Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> AddAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken);
    Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> UpdateAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
