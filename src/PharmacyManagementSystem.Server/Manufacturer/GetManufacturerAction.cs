using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Manufacturer;

namespace PharmacyManagementSystem.Server.Manufacturer;

public class GetManufacturerAction(ILogger<GetManufacturerAction> logger, IManufacturerRepository repository) : IGetManufacturerAction
{
    private readonly ILogger<GetManufacturerAction> _logger = logger;
    private readonly IManufacturerRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.Manufacturer.Manufacturer>?> GetByFilterCriteriaAsync(ManufacturerFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting manufacturers by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} manufacturers.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Manufacturer.Manufacturer?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting manufacturer by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved manufacturer with id: {Id}.", id);

        return result;
    }
}
