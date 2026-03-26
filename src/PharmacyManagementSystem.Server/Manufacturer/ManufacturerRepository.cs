using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Manufacturer;

namespace PharmacyManagementSystem.Server.Manufacturer;

public class ManufacturerRepository(ILogger<ManufacturerRepository> logger, IManufacturerStorageClient storageClient) : IManufacturerRepository
{
    private readonly ILogger<ManufacturerRepository> _logger = logger;
    private readonly IManufacturerStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Manufacturer.Manufacturer>?> GetByFilterCriteriaAsync(ManufacturerFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting manufacturers by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} manufacturers.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Manufacturer.Manufacturer?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting manufacturer by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved manufacturer with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Manufacturer.Manufacturer?> AddAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        _logger.LogDebug("Repository: Adding manufacturer with name: {Name}.", manufacturer.Name);

        var result = await _storageClient.AddAsync(manufacturer, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added manufacturer with name: {Name}.", manufacturer.Name);

        return result;
    }

    public async Task<Common.Manufacturer.Manufacturer?> UpdateAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        _logger.LogDebug("Repository: Updating manufacturer with id: {Id}.", manufacturer.Id);

        var result = await _storageClient.UpdateAsync(manufacturer, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated manufacturer with id: {Id}.", manufacturer.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing manufacturer with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed manufacturer with id: {Id}.", id);
    }
}
