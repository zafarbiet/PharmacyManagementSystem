using PharmacyManagementSystem.Common.DebtReminder;

namespace PharmacyManagementSystem.Server.DebtReminder;

public interface IDebtReminderStorageClient
{
    Task<IReadOnlyCollection<Common.DebtReminder.DebtReminder>?> GetByFilterCriteriaAsync(DebtReminderFilter filter, CancellationToken cancellationToken);
    Task<Common.DebtReminder.DebtReminder?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.DebtReminder.DebtReminder?> AddAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken);
    Task<Common.DebtReminder.DebtReminder?> UpdateAsync(Common.DebtReminder.DebtReminder? debtReminder, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
