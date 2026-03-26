namespace PharmacyManagementSystem.Server.MenuItem;

public interface ISaveMenuItemAction
{
    Task<Common.MenuItem.MenuItem?> AddAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken);
    Task<Common.MenuItem.MenuItem?> UpdateAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
