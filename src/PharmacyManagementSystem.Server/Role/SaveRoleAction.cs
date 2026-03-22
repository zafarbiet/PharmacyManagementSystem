using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Role;

public class SaveRoleAction(ILogger<SaveRoleAction> logger, IRoleRepository repository) : ISaveRoleAction
{
    private readonly ILogger<SaveRoleAction> _logger = logger;
    private readonly IRoleRepository _repository = repository;

    public async Task<Common.Role.Role?> AddAsync(Common.Role.Role? role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role);

        if (string.IsNullOrWhiteSpace(role.Name))
            throw new BadRequestException("Role Name is required.");

        role.UpdatedBy = "system";

        _logger.LogDebug("Adding new role with name: {Name}.", role.Name);

        var result = await _repository.AddAsync(role, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added role with name: {Name}.", role.Name);

        return result;
    }

    public async Task<Common.Role.Role?> UpdateAsync(Common.Role.Role? role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role);

        if (string.IsNullOrWhiteSpace(role.Name))
            throw new BadRequestException("Role Name is required.");

        role.UpdatedBy = "system";

        _logger.LogDebug("Updating role with id: {Id}.", role.Id);

        var result = await _repository.UpdateAsync(role, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated role with id: {Id}.", role.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing role with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed role with id: {Id}.", id);
    }
}
