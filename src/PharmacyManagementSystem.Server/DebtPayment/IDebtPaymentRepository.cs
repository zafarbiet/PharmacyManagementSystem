using PharmacyManagementSystem.Common.DebtPayment;

namespace PharmacyManagementSystem.Server.DebtPayment;

public interface IDebtPaymentRepository
{
    Task<IReadOnlyCollection<Common.DebtPayment.DebtPayment>?> GetByFilterCriteriaAsync(DebtPaymentFilter filter, CancellationToken cancellationToken);
    Task<Common.DebtPayment.DebtPayment?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DebtPayment.DebtPayment?> AddAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken);
    Task<Common.DebtPayment.DebtPayment?> UpdateAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
