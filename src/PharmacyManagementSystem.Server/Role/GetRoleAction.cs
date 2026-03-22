using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Role;

namespace PharmacyManagementSystem.Server.Role;

public class GetRoleAction(ILogger<GetRoleAction> logger, IRoleRepository repository) : IGetRoleAction
{
    private readonly ILogger<GetRoleAction> _logger = logger;
    private readonly IRoleRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Role.Role>?> GetByFilterCriteriaAsync(RoleFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting roles by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} roles.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Role.Role?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting role by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved role with id: {Id}.", id);

        return result;
    }
}
