using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;

namespace PharmacyManagementSystem.Server.DrugInventoryRackAssignment;

public class DrugInventoryRackAssignmentRepository(ILogger<DrugInventoryRackAssignmentRepository> logger, IDrugInventoryRackAssignmentStorageClient storageClient) : IDrugInventoryRackAssignmentRepository
{
    private readonly ILogger<DrugInventoryRackAssignmentRepository> _logger = logger;
    private readonly IDrugInventoryRackAssignmentStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>?> GetByFilterCriteriaAsync(DrugInventoryRackAssignmentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting drug inventory rack assignments by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} drug inventory rack assignments.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting drug inventory rack assignment by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved drug inventory rack assignment with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> AddAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);

        _logger.LogDebug("Repository: Adding drug inventory rack assignment for drug inventory: {DrugInventoryId}.", drugInventoryRackAssignment.DrugInventoryId);

        var result = await _storageClient.AddAsync(drugInventoryRackAssignment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added drug inventory rack assignment for drug inventory: {DrugInventoryId}.", drugInventoryRackAssignment.DrugInventoryId);

        return result;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> UpdateAsync(Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment? drugInventoryRackAssignment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugInventoryRackAssignment);

        _logger.LogDebug("Repository: Updating drug inventory rack assignment with id: {Id}.", drugInventoryRackAssignment.Id);

        var result = await _storageClient.UpdateAsync(drugInventoryRackAssignment, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated drug inventory rack assignment with id: {Id}.", drugInventoryRackAssignment.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing drug inventory rack assignment with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed drug inventory rack assignment with id: {Id}.", id);
    }
}
