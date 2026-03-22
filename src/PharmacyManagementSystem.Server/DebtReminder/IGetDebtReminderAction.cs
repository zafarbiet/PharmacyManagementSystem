using PharmacyManagementSystem.Common.DebtReminder;

namespace PharmacyManagementSystem.Server.DebtReminder;

public interface IGetDebtReminderAction
{
    Task<IReadOnlyCollection<Common.DebtReminder.DebtReminder>?> GetByFilterCriteriaAsync(DebtReminderFilter filter, CancellationToken cancellationToken);
    Task<Common.DebtReminder.DebtReminder?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
