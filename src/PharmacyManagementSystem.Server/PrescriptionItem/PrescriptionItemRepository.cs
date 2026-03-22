using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PrescriptionItem;

namespace PharmacyManagementSystem.Server.PrescriptionItem;

public class PrescriptionItemRepository(ILogger<PrescriptionItemRepository> logger, IPrescriptionItemStorageClient storageClient) : IPrescriptionItemRepository
{
    private readonly ILogger<PrescriptionItemRepository> _logger = logger;
    private readonly IPrescriptionItemStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.PrescriptionItem.PrescriptionItem>?> GetByFilterCriteriaAsync(PrescriptionItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting prescription items by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} prescription items.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting prescription item by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved prescription item with id: {Id}.", id);

        return result;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> AddAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);

        _logger.LogDebug("Repository: Adding prescription item for PrescriptionId: {PrescriptionId}.", prescriptionItem.PrescriptionId);

        var result = await _storageClient.AddAsync(prescriptionItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added prescription item for PrescriptionId: {PrescriptionId}.", prescriptionItem.PrescriptionId);

        return result;
    }

    public async Task<Common.PrescriptionItem.PrescriptionItem?> UpdateAsync(Common.PrescriptionItem.PrescriptionItem? prescriptionItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prescriptionItem);

        _logger.LogDebug("Repository: Updating prescription item with id: {Id}.", prescriptionItem.Id);

        var result = await _storageClient.UpdateAsync(prescriptionItem, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated prescription item with id: {Id}.", prescriptionItem.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing prescription item with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed prescription item with id: {Id}.", id);
    }
}
