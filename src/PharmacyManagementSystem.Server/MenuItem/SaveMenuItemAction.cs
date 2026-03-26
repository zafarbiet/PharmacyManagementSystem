using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.MenuItem;

public class SaveMenuItemAction(ILogger<SaveMenuItemAction> logger, IMenuItemRepository repository) : ISaveMenuItemAction
{
    private readonly ILogger<SaveMenuItemAction> _logger = logger;
    private readonly IMenuItemRepository _repository = repository;

    public async Task<Common.MenuItem.MenuItem?> AddAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        if (string.IsNullOrWhiteSpace(menuItem.Key))
            throw new BadRequestException("MenuItem Key is required.");

        if (string.IsNullOrWhiteSpace(menuItem.Label))
            throw new BadRequestException("MenuItem Label is required.");

        menuItem.UpdatedBy = "system";

        _logger.LogDebug("Adding new menu item with key: {Key}.", menuItem.Key);
        var result = await _repository.AddAsync(menuItem, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Added menu item with key: {Key}.", menuItem.Key);
        return result;
    }

    public async Task<Common.MenuItem.MenuItem?> UpdateAsync(Common.MenuItem.MenuItem? menuItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        if (string.IsNullOrWhiteSpace(menuItem.Key))
            throw new BadRequestException("MenuItem Key is required.");

        if (string.IsNullOrWhiteSpace(menuItem.Label))
            throw new BadRequestException("MenuItem Label is required.");

        menuItem.UpdatedBy = "system";

        _logger.LogDebug("Updating menu item with id: {Id}.", menuItem.Id);
        var result = await _repository.UpdateAsync(menuItem, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Updated menu item with id: {Id}.", menuItem.Id);
        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("Removing menu item with id: {Id}.", id);
        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("Removed menu item with id: {Id}.", id);
    }
}
