using PharmacyManagementSystem.Common.MenuItem;

namespace PharmacyManagementSystem.Server.MenuItem;

public interface IGetMenuItemAction
{
    Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetByFilterCriteriaAsync(MenuItemFilter filter, CancellationToken cancellationToken);
    Task<Common.MenuItem.MenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetForUserAsync(string username, CancellationToken cancellationToken);
}
