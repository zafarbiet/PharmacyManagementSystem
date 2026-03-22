using PharmacyManagementSystem.Common.CustomerInvoiceItem;

namespace PharmacyManagementSystem.Server.CustomerInvoiceItem;

public interface IGetCustomerInvoiceItemAction
{
    Task<IReadOnlyCollection<Common.CustomerInvoiceItem.CustomerInvoiceItem>?> GetByFilterCriteriaAsync(CustomerInvoiceItemFilter filter, CancellationToken cancellationToken);
    Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
