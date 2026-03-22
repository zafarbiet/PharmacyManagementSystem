using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Rack;

namespace PharmacyManagementSystem.Server.Rack;

public class GetRackAction(ILogger<GetRackAction> logger, IRackRepository repository) : IGetRackAction
{
    private readonly ILogger<GetRackAction> _logger = logger;
    private readonly IRackRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Rack.Rack>?> GetByFilterCriteriaAsync(RackFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting racks by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} racks.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Rack.Rack?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting rack by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved rack with id: {Id}.", id);

        return result;
    }
}
