using PharmacyManagementSystem.Common.QuotationRequestItem;

namespace PharmacyManagementSystem.Server.QuotationRequestItem;

public interface IGetQuotationRequestItemAction
{
    Task<IReadOnlyCollection<Common.QuotationRequestItem.QuotationRequestItem>?> GetByFilterCriteriaAsync(QuotationRequestItemFilter filter, CancellationToken cancellationToken);
    Task<Common.QuotationRequestItem.QuotationRequestItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
