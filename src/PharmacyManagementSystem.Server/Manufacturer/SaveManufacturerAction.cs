using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Manufacturer;

public class SaveManufacturerAction(ILogger<SaveManufacturerAction> logger, IManufacturerRepository repository) : ISaveManufacturerAction
{
    private readonly ILogger<SaveManufacturerAction> _logger = logger;
    private readonly IManufacturerRepository _repository = repository;

    public async Task<Common.Manufacturer.Manufacturer?> AddAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        if (string.IsNullOrWhiteSpace(manufacturer.Name))
            throw new BadRequestException("Manufacturer Name is required.");

        manufacturer.UpdatedBy = "system";

        _logger.LogDebug("Adding new manufacturer with name: {Name}.", manufacturer.Name);

        var result = await _repository.AddAsync(manufacturer, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added manufacturer with name: {Name}.", manufacturer.Name);

        return result;
    }

    public async Task<Common.Manufacturer.Manufacturer?> UpdateAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        if (string.IsNullOrWhiteSpace(manufacturer.Name))
            throw new BadRequestException("Manufacturer Name is required.");

        manufacturer.UpdatedBy = "system";

        _logger.LogDebug("Updating manufacturer with id: {Id}.", manufacturer.Id);

        var result = await _repository.UpdateAsync(manufacturer, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated manufacturer with id: {Id}.", manufacturer.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing manufacturer with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed manufacturer with id: {Id}.", id);
    }
}
