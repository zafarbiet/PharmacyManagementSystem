using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.UserRole;

namespace PharmacyManagementSystem.Server.UserRole;

public class GetUserRoleAction(ILogger<GetUserRoleAction> logger, IUserRoleRepository repository) : IGetUserRoleAction
{
    private readonly ILogger<GetUserRoleAction> _logger = logger;
    private readonly IUserRoleRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.UserRole.UserRole>?> GetByFilterCriteriaAsync(UserRoleFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting user roles by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} user roles.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.UserRole.UserRole?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting user role by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved user role with id: {Id}.", id);

        return result;
    }
}
