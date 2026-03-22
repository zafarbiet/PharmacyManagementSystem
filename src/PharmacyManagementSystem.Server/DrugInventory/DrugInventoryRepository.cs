using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventory;

namespace PharmacyManagementSystem.Server.DrugInventory;

public class DrugInventoryRepository(ILogger<DrugInventoryRepository> logger, IDrugInventoryStorageClient storageClient) : IDrugInventoryRepository
{
    private readonly ILogger<DrugInventoryRepository> _logger = logger;
    private readonly IDrugInventoryStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DrugInventory.DrugInventory>?> GetByFilterCriteriaAsync(DrugInventoryFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting drug inventories by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} drug inventories.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugInventory.DrugInventory?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting drug inventory by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved drug inventory with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DrugInventory.DrugInventory?> AddAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        _logger.LogDebug("Repository: Adding drug inventory for DrugId: {DrugId}.", drugInventory.DrugId);

        var result = await _storageClient.AddAsync(drugInventory, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added drug inventory for DrugId: {DrugId}.", drugInventory.DrugId);

        return result;
    }

    public async Task<Common.DrugInventory.DrugInventory?> UpdateAsync(Common.DrugInventory.DrugInventory? drugInventory, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventory);

        _logger.LogDebug("Repository: Updating drug inventory with id: {Id}.", drugInventory.Id);

        var result = await _storageClient.UpdateAsync(drugInventory, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated drug inventory with id: {Id}.", drugInventory.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing drug inventory with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed drug inventory with id: {Id}.", id);
    }
}
