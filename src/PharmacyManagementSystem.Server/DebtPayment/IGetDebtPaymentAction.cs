using PharmacyManagementSystem.Common.DebtPayment;

namespace PharmacyManagementSystem.Server.DebtPayment;

public interface IGetDebtPaymentAction
{
    Task<IReadOnlyCollection<Common.DebtPayment.DebtPayment>?> GetByFilterCriteriaAsync(DebtPaymentFilter filter, CancellationToken cancellationToken);
    Task<Common.DebtPayment.DebtPayment?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
