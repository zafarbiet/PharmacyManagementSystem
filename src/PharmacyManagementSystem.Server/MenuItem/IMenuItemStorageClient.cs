using PharmacyManagementSystem.Common.MenuItem;

namespace PharmacyManagementSystem.Server.MenuItem;

public interface IMenuItemStorageClient
{
    Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetByFilterCriteriaAsync(MenuItemFilter filter, CancellationToken cancellationToken);
    Task<Common.MenuItem.MenuItem?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Common.MenuItem.MenuItem>?> GetForUserAsync(string username, CancellationToken cancellationToken);
    Task<Common.MenuItem.MenuItem?> AddAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken);
    Task<Common.MenuItem.MenuItem?> UpdateAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
