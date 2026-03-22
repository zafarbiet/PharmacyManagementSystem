using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;

namespace PharmacyManagementSystem.Server.VendorExpiryReturnRequest;

public interface IGetVendorExpiryReturnRequestAction
{
    Task<IReadOnlyCollection<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>?> GetByFilterCriteriaAsync(VendorExpiryReturnRequestFilter filter, CancellationToken cancellationToken);
    Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
