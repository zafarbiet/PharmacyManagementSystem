using PharmacyManagementSystem.Common.QuotationItem;
namespace PharmacyManagementSystem.Server.QuotationItem;
public interface IQuotationItemRepository
{
    Task<IReadOnlyCollection<Common.QuotationItem.QuotationItem>?> GetByFilterCriteriaAsync(QuotationItemFilter filter, CancellationToken cancellationToken);
    Task<Common.QuotationItem.QuotationItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.QuotationItem.QuotationItem?> AddAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken);
    Task<Common.QuotationItem.QuotationItem?> UpdateAsync(Common.QuotationItem.QuotationItem? quotationItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
