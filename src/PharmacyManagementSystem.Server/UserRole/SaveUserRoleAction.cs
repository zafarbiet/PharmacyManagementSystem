using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.UserRole;

public class SaveUserRoleAction(ILogger<SaveUserRoleAction> logger, IUserRoleRepository repository) : ISaveUserRoleAction
{
    private readonly ILogger<SaveUserRoleAction> _logger = logger;
    private readonly IUserRoleRepository _repository = repository;

    public async Task<Common.UserRole.UserRole?> AddAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userRole);

        if (userRole.UserId == Guid.Empty)
            throw new BadRequestException("UserRole UserId is required.");

        if (userRole.RoleId == Guid.Empty)
            throw new BadRequestException("UserRole RoleId is required.");

        userRole.UpdatedBy = "system";

        _logger.LogDebug("Adding new user role for user: {UserId}.", userRole.UserId);

        var result = await _repository.AddAsync(userRole, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added user role for user: {UserId}.", userRole.UserId);

        return result;
    }

    public async Task<Common.UserRole.UserRole?> UpdateAsync(Common.UserRole.UserRole? userRole, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userRole);

        if (userRole.UserId == Guid.Empty)
            throw new BadRequestException("UserRole UserId is required.");

        if (userRole.RoleId == Guid.Empty)
            throw new BadRequestException("UserRole RoleId is required.");

        userRole.UpdatedBy = "system";

        _logger.LogDebug("Updating user role with id: {Id}.", userRole.Id);

        var result = await _repository.UpdateAsync(userRole, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated user role with id: {Id}.", userRole.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing user role with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed user role with id: {Id}.", id);
    }
}
