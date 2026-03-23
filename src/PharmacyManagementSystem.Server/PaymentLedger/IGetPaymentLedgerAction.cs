using PharmacyManagementSystem.Common.PaymentLedger;

namespace PharmacyManagementSystem.Server.PaymentLedger;

public interface IGetPaymentLedgerAction
{
    Task<IReadOnlyCollection<Common.PaymentLedger.PaymentLedger>?> GetByFilterCriteriaAsync(PaymentLedgerFilter filter, CancellationToken cancellationToken);
    Task<Common.PaymentLedger.PaymentLedger?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
