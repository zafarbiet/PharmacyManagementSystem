using PharmacyManagementSystem.Common.CustomerInvoice;

namespace PharmacyManagementSystem.Server.CustomerInvoice;

public interface ICustomerInvoiceStorageClient
{
    Task<IReadOnlyCollection<Common.CustomerInvoice.CustomerInvoice>?> GetByFilterCriteriaAsync(CustomerInvoiceFilter filter, CancellationToken cancellationToken);
    Task<Common.CustomerInvoice.CustomerInvoice?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.CustomerInvoice.CustomerInvoice?> AddAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken);
    Task<Common.CustomerInvoice.CustomerInvoice?> UpdateAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
    Task<string> GetNextInvoiceNumberAsync(CancellationToken cancellationToken);
}
