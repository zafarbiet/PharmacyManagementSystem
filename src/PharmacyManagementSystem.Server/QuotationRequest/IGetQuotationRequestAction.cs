using PharmacyManagementSystem.Common.QuotationRequest;

namespace PharmacyManagementSystem.Server.QuotationRequest;

public interface IGetQuotationRequestAction
{
    Task<IReadOnlyCollection<Common.QuotationRequest.QuotationRequest>?> GetByFilterCriteriaAsync(QuotationRequestFilter filter, CancellationToken cancellationToken);
    Task<Common.QuotationRequest.QuotationRequest?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
