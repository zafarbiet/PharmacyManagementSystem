using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Branch;

namespace PharmacyManagementSystem.Server.Branch;

public class BranchRepository(ILogger<BranchRepository> logger, IBranchStorageClient storageClient) : IBranchRepository
{
    private readonly ILogger<BranchRepository> _logger = logger;
    private readonly IBranchStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Branch.Branch>?> GetByFilterCriteriaAsync(BranchFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting branches by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} branches.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Branch.Branch?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting branch by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved branch with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Branch.Branch?> AddAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(branch);

        _logger.LogDebug("Repository: Adding branch with name: {Name}.", branch.Name);

        var result = await _storageClient.AddAsync(branch, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added branch with name: {Name}.", branch.Name);

        return result;
    }

    public async Task<Common.Branch.Branch?> UpdateAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(branch);

        _logger.LogDebug("Repository: Updating branch with id: {Id}.", branch.Id);

        var result = await _storageClient.UpdateAsync(branch, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated branch with id: {Id}.", branch.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing branch with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed branch with id: {Id}.", id);
    }
}
