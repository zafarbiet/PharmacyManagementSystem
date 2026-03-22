namespace PharmacyManagementSystem.Server.DebtPayment;

public interface ISaveDebtPaymentAction
{
    Task<Common.DebtPayment.DebtPayment?> AddAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken);
    Task<Common.DebtPayment.DebtPayment?> UpdateAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
