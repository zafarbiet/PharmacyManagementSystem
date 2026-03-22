using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Rack;

public class SaveRackAction(ILogger<SaveRackAction> logger, IRackRepository repository) : ISaveRackAction
{
    private readonly ILogger<SaveRackAction> _logger = logger;
    private readonly IRackRepository _repository = repository;

    public async Task<Common.Rack.Rack?> AddAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rack);

        if (rack.StorageZoneId == Guid.Empty)
            throw new BadRequestException("Rack StorageZoneId is required.");

        if (string.IsNullOrWhiteSpace(rack.Label))
            throw new BadRequestException("Rack Label is required.");

        rack.UpdatedBy = "system";

        _logger.LogDebug("Adding new rack with label: {Label}.", rack.Label);

        var result = await _repository.AddAsync(rack, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added rack with label: {Label}.", rack.Label);

        return result;
    }

    public async Task<Common.Rack.Rack?> UpdateAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rack);

        if (rack.StorageZoneId == Guid.Empty)
            throw new BadRequestException("Rack StorageZoneId is required.");

        if (string.IsNullOrWhiteSpace(rack.Label))
            throw new BadRequestException("Rack Label is required.");

        rack.UpdatedBy = "system";

        _logger.LogDebug("Updating rack with id: {Id}.", rack.Id);

        var result = await _repository.UpdateAsync(rack, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated rack with id: {Id}.", rack.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing rack with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed rack with id: {Id}.", id);
    }
}
