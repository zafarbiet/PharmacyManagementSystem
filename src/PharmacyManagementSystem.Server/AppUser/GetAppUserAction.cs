using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.AppUser;

namespace PharmacyManagementSystem.Server.AppUser;

public class GetAppUserAction(ILogger<GetAppUserAction> logger, IAppUserRepository repository) : IGetAppUserAction
{
    private readonly ILogger<GetAppUserAction> _logger = logger;
    private readonly IAppUserRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.AppUser.AppUser>?> GetByFilterCriteriaAsync(AppUserFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting app users by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} app users.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.AppUser.AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting app user by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved app user with id: {Id}.", id);

        return result;
    }
}
