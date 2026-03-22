namespace PharmacyManagementSystem.Server.CustomerInvoiceItem;

public interface ISaveCustomerInvoiceItemAction
{
    Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> AddAsync(Common.CustomerInvoiceItem.CustomerInvoiceItem? customerInvoiceItem, CancellationToken cancellationToken);
    Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> UpdateAsync(Common.CustomerInvoiceItem.CustomerInvoiceItem? customerInvoiceItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
