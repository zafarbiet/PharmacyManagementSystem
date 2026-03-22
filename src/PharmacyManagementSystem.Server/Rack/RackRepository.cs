using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Rack;

namespace PharmacyManagementSystem.Server.Rack;

public class RackRepository(ILogger<RackRepository> logger, IRackStorageClient storageClient) : IRackRepository
{
    private readonly ILogger<RackRepository> _logger = logger;
    private readonly IRackStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.Rack.Rack>?> GetByFilterCriteriaAsync(RackFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting racks by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} racks.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.Rack.Rack?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting rack by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved rack with id: {Id}.", id);

        return result;
    }

    public async Task<Common.Rack.Rack?> AddAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rack);

        _logger.LogDebug("Repository: Adding rack with label: {Label}.", rack.Label);

        var result = await _storageClient.AddAsync(rack, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added rack with label: {Label}.", rack.Label);

        return result;
    }

    public async Task<Common.Rack.Rack?> UpdateAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rack);

        _logger.LogDebug("Repository: Updating rack with id: {Id}.", rack.Id);

        var result = await _storageClient.UpdateAsync(rack, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated rack with id: {Id}.", rack.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing rack with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed rack with id: {Id}.", id);
    }
}
