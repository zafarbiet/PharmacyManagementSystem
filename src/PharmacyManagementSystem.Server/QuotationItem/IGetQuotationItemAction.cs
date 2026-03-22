using PharmacyManagementSystem.Common.QuotationItem;
namespace PharmacyManagementSystem.Server.QuotationItem;
public interface IGetQuotationItemAction
{
    Task<IReadOnlyCollection<Common.QuotationItem.QuotationItem>?> GetByFilterCriteriaAsync(QuotationItemFilter filter, CancellationToken cancellationToken);
    Task<Common.QuotationItem.QuotationItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
