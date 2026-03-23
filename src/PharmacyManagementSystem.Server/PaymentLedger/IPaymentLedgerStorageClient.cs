using PharmacyManagementSystem.Common.PaymentLedger;

namespace PharmacyManagementSystem.Server.PaymentLedger;

public interface IPaymentLedgerStorageClient
{
    Task<IReadOnlyCollection<Common.PaymentLedger.PaymentLedger>?> GetByFilterCriteriaAsync(PaymentLedgerFilter filter, CancellationToken cancellationToken);
    Task<Common.PaymentLedger.PaymentLedger?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.PaymentLedger.PaymentLedger?> AddAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken);
    Task<Common.PaymentLedger.PaymentLedger?> UpdateAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
