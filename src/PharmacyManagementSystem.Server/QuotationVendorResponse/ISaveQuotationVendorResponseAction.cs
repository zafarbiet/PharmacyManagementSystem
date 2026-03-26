namespace PharmacyManagementSystem.Server.QuotationVendorResponse;

public interface ISaveQuotationVendorResponseAction
{
    Task<Common.QuotationVendorResponse.QuotationVendorResponse?> AddAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken);
    Task<Common.QuotationVendorResponse.QuotationVendorResponse?> UpdateAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
