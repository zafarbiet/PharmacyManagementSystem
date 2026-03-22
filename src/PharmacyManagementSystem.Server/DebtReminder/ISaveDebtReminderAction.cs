namespace PharmacyManagementSystem.Server.DebtReminder;

public interface ISaveDebtReminderAction
{
    Task<Common.DebtReminder.DebtReminder?> AddAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken);
    Task<Common.DebtReminder.DebtReminder?> UpdateAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
