using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.AppUser;

namespace PharmacyManagementSystem.Server.AppUser;

public class AppUserRepository(ILogger<AppUserRepository> logger, IAppUserStorageClient storageClient) : IAppUserRepository
{
    private readonly ILogger<AppUserRepository> _logger = logger;
    private readonly IAppUserStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.AppUser.AppUser>?> GetByFilterCriteriaAsync(AppUserFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting app users by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} app users.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.AppUser.AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting app user by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved app user with id: {Id}.", id);

        return result;
    }

    public async Task<Common.AppUser.AppUser?> AddAsync(Common.AppUser.AppUser? appUser, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(appUser);

        _logger.LogDebug("Repository: Adding app user with username: {Username}.", appUser.Username);

        var result = await _storageClient.AddAsync(appUser, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added app user with username: {Username}.", appUser.Username);

        return result;
    }

    public async Task<Common.AppUser.AppUser?> UpdateAsync(Common.AppUser.AppUser? appUser, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(appUser);

        _logger.LogDebug("Repository: Updating app user with id: {Id}.", appUser.Id);

        var result = await _storageClient.UpdateAsync(appUser, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated app user with id: {Id}.", appUser.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing app user with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed app user with id: {Id}.", id);
    }
}
