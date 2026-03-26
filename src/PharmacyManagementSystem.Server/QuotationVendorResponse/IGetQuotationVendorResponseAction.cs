using PharmacyManagementSystem.Common.QuotationVendorResponse;

namespace PharmacyManagementSystem.Server.QuotationVendorResponse;

public interface IGetQuotationVendorResponseAction
{
    Task<IReadOnlyCollection<Common.QuotationVendorResponse.QuotationVendorResponse>?> GetByFilterCriteriaAsync(QuotationVendorResponseFilter filter, CancellationToken cancellationToken);
    Task<Common.QuotationVendorResponse.QuotationVendorResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
