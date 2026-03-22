namespace PharmacyManagementSystem.Server.CustomerInvoice;

public interface ISaveCustomerInvoiceAction
{
    Task<Common.CustomerInvoice.CustomerInvoice?> AddAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken);
    Task<Common.CustomerInvoice.CustomerInvoice?> UpdateAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
