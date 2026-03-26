using PharmacyManagementSystem.Common.QuotationVendorResponse;

namespace PharmacyManagementSystem.Server.QuotationVendorResponse;

public interface IQuotationVendorResponseRepository
{
    Task<IReadOnlyCollection<Common.QuotationVendorResponse.QuotationVendorResponse>?> GetByFilterCriteriaAsync(QuotationVendorResponseFilter filter, CancellationToken cancellationToken);
    Task<Common.QuotationVendorResponse.QuotationVendorResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.QuotationVendorResponse.QuotationVendorResponse?> AddAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken);
    Task<Common.QuotationVendorResponse.QuotationVendorResponse?> UpdateAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
