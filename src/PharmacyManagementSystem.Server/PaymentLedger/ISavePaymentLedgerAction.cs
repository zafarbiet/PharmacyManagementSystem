namespace PharmacyManagementSystem.Server.PaymentLedger;

public interface ISavePaymentLedgerAction
{
    Task<Common.PaymentLedger.PaymentLedger?> AddAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken);
    Task<Common.PaymentLedger.PaymentLedger?> UpdateAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
    Task<Common.PaymentLedger.PaymentLedger?> RecordPaymentAsync(Guid ledgerId, decimal paymentAmount, CancellationToken cancellationToken);
}
