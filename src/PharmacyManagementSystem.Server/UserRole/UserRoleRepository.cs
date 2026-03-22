using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.UserRole;

namespace PharmacyManagementSystem.Server.UserRole;

public class UserRoleRepository(ILogger<UserRoleRepository> logger, IUserRoleStorageClient storageClient) : IUserRoleRepository
{
    private readonly ILogger<UserRoleRepository> _logger = logger;
    private readonly IUserRoleStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.UserRole.UserRole>?> GetByFilterCriteriaAsync(UserRoleFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting user roles by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} user roles.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.UserRole.UserRole?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting user role by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved user role with id: {Id}.", id);

        return result;
    }

    public async Task<Common.UserRole.UserRole?> AddAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userRole);

        _logger.LogDebug("Repository: Adding user role for user: {UserId}.", userRole.UserId);

        var result = await _storageClient.AddAsync(userRole, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added user role for user: {UserId}.", userRole.UserId);

        return result;
    }

    public async Task<Common.UserRole.UserRole?> UpdateAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userRole);

        _logger.LogDebug("Repository: Updating user role with id: {Id}.", userRole.Id);

        var result = await _storageClient.UpdateAsync(userRole, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated user role with id: {Id}.", userRole.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing user role with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed user role with id: {Id}.", id);
    }
}
