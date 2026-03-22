using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugPricing;

namespace PharmacyManagementSystem.Server.DrugPricing;

public class DrugPricingRepository(ILogger<DrugPricingRepository> logger, IDrugPricingStorageClient storageClient) : IDrugPricingRepository
{
    private readonly ILogger<DrugPricingRepository> _logger = logger;
    private readonly IDrugPricingStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.DrugPricing.DrugPricing>?> GetByFilterCriteriaAsync(DrugPricingFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting drug pricings by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} drug pricings.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugPricing.DrugPricing?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting drug pricing by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved drug pricing with id: {Id}.", id);

        return result;
    }

    public async Task<Common.DrugPricing.DrugPricing?> AddAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);

        _logger.LogDebug("Repository: Adding drug pricing for DrugId: {DrugId}.", drugPricing.DrugId);

        var result = await _storageClient.AddAsync(drugPricing, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added drug pricing for DrugId: {DrugId}.", drugPricing.DrugId);

        return result;
    }

    public async Task<Common.DrugPricing.DrugPricing?> UpdateAsync(Common.DrugPricing.DrugPricing? drugPricing, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugPricing);

        _logger.LogDebug("Repository: Updating drug pricing with id: {Id}.", drugPricing.Id);

        var result = await _storageClient.UpdateAsync(drugPricing, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated drug pricing with id: {Id}.", drugPricing.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing drug pricing with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed drug pricing with id: {Id}.", id);
    }
}
