using PharmacyManagementSystem.Common.QuotationRequest;

namespace PharmacyManagementSystem.Server.QuotationRequest;

public interface IQuotationRequestRepository
{
    Task<IReadOnlyCollection<Common.QuotationRequest.QuotationRequest>?> GetByFilterCriteriaAsync(QuotationRequestFilter filter, CancellationToken cancellationToken);
    Task<Common.QuotationRequest.QuotationRequest?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.QuotationRequest.QuotationRequest?> AddAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken);
    Task<Common.QuotationRequest.QuotationRequest?> UpdateAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
