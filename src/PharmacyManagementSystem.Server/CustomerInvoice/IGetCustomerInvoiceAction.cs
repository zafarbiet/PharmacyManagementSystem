using PharmacyManagementSystem.Common.CustomerInvoice;

namespace PharmacyManagementSystem.Server.CustomerInvoice;

public interface IGetCustomerInvoiceAction
{
    Task<IReadOnlyCollection<Common.CustomerInvoice.CustomerInvoice>?> GetByFilterCriteriaAsync(CustomerInvoiceFilter filter, CancellationToken cancellationToken);
    Task<Common.CustomerInvoice.CustomerInvoice?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
