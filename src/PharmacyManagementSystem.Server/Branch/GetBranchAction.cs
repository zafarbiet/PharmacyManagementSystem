using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Branch;

namespace PharmacyManagementSystem.Server.Branch;

public class GetBranchAction(ILogger<GetBranchAction> logger, IBranchRepository repository) : IGetBranchAction
{
    private readonly ILogger<GetBranchAction> _logger = logger;
    private readonly IBranchRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Branch.Branch>?> GetByFilterCriteriaAsync(BranchFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting branches by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} branches.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Branch.Branch?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting branch by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved branch with id: {Id}.", id);

        return result;
    }
}
