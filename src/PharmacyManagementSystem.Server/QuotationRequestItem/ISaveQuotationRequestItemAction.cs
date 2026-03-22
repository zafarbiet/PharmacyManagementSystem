namespace PharmacyManagementSystem.Server.QuotationRequestItem;

public interface ISaveQuotationRequestItemAction
{
    Task<Common.QuotationRequestItem.QuotationRequestItem?> AddAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken);
    Task<Common.QuotationRequestItem.QuotationRequestItem?> UpdateAsync(Common.QuotationRequestItem.QuotationRequestItem? quotationRequestItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
