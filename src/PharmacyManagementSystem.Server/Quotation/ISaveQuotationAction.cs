namespace PharmacyManagementSystem.Server.Quotation;
public interface ISaveQuotationAction
{
    Task<Common.Quotation.Quotation?> AddAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken);
    Task<Common.Quotation.Quotation?> UpdateAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
    Task<Common.PurchaseOrder.PurchaseOrder?> AcceptAsync(Guid quotationId, Guid? branchId, CancellationToken cancellationToken);
}
