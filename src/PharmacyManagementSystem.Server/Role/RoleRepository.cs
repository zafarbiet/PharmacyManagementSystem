using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Role;

namespace PharmacyManagementSystem.Server.Role;

public class RoleRepository(ILogger<RoleRepository> logger, IRoleStorageClient storageClient) : IRoleRepository
{
    private readonly ILogger<RoleRepository> _logger = logger;
    private readonly IRoleStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Role.Role>?> GetByFilterCriteriaAsync(RoleFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting roles by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} roles.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Role.Role?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting role by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved role with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Role.Role?> AddAsync(Common.Role.Role? role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role);

        _logger.LogDebug("Repository: Adding role with name: {Name}.", role.Name);

        var result = await _storageClient.AddAsync(role, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added role with name: {Name}.", role.Name);

        return result;
    }

    public async Task<Common.Role.Role?> UpdateAsync(Common.Role.Role? role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role);

        _logger.LogDebug("Repository: Updating role with id: {Id}.", role.Id);

        var result = await _storageClient.UpdateAsync(role, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated role with id: {Id}.", role.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing role with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed role with id: {Id}.", id);
    }
}
