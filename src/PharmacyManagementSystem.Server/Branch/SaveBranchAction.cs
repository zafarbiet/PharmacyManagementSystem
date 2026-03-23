using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Branch;

public class SaveBranchAction(ILogger<SaveBranchAction> logger, IBranchRepository repository) : ISaveBranchAction
{
    private readonly ILogger<SaveBranchAction> _logger = logger;
    private readonly IBranchRepository _repository = repository;

    public async Task<Common.Branch.Branch?> AddAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(branch);

        if (string.IsNullOrWhiteSpace(branch.Name))
            throw new BadRequestException("Branch Name is required.");

        branch.UpdatedBy = "system";

        _logger.LogDebug("Adding new branch with name: {Name}.", branch.Name);

        var result = await _repository.AddAsync(branch, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added branch with name: {Name}.", branch.Name);

        return result;
    }

    public async Task<Common.Branch.Branch?> UpdateAsync(Common.Branch.Branch? branch, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(branch);

        if (string.IsNullOrWhiteSpace(branch.Name))
            throw new BadRequestException("Branch Name is required.");

        branch.UpdatedBy = "system";

        _logger.LogDebug("Updating branch with id: {Id}.", branch.Id);

        var result = await _repository.UpdateAsync(branch, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated branch with id: {Id}.", branch.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing branch with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed branch with id: {Id}.", id);
    }
}
