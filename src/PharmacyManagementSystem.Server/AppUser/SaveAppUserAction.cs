using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.AppUser;

public class SaveAppUserAction(ILogger<SaveAppUserAction> logger, IAppUserRepository repository) : ISaveAppUserAction
{
    private readonly ILogger<SaveAppUserAction> _logger = logger;
    private readonly IAppUserRepository _repository = repository;

    public async Task<Common.AppUser.AppUser?> AddAsync(Common.AppUser.AppUser? appUser, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(appUser);

        if (string.IsNullOrWhiteSpace(appUser.Username))
            throw new BadRequestException("AppUser Username is required.");

        if (string.IsNullOrWhiteSpace(appUser.FullName))
            throw new BadRequestException("AppUser FullName is required.");

        if (string.IsNullOrWhiteSpace(appUser.PasswordHash))
            throw new BadRequestException("AppUser PasswordHash is required.");

        appUser.UpdatedBy = "system";

        _logger.LogDebug("Adding new app user with username: {Username}.", appUser.Username);

        var result = await _repository.AddAsync(appUser, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added app user with username: {Username}.", appUser.Username);

        return result;
    }

    public async Task<Common.AppUser.AppUser?> UpdateAsync(Common.AppUser.AppUser? appUser, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(appUser);

        if (string.IsNullOrWhiteSpace(appUser.Username))
            throw new BadRequestException("AppUser Username is required.");

        if (string.IsNullOrWhiteSpace(appUser.FullName))
            throw new BadRequestException("AppUser FullName is required.");

        if (string.IsNullOrWhiteSpace(appUser.PasswordHash))
            throw new BadRequestException("AppUser PasswordHash is required.");

        appUser.UpdatedBy = "system";

        _logger.LogDebug("Updating app user with id: {Id}.", appUser.Id);

        var result = await _repository.UpdateAsync(appUser, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated app user with id: {Id}.", appUser.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing app user with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed app user with id: {Id}.", id);
    }
}
