using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DrugUsage;

public class SaveDrugUsageAction(ILogger<SaveDrugUsageAction> logger, IDrugUsageRepository repository) : ISaveDrugUsageAction
{
    private readonly ILogger<SaveDrugUsageAction> _logger = logger;
    private readonly IDrugUsageRepository _repository = repository;

    public async Task<Common.DrugUsage.DrugUsage?> AddAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);

        if (drugUsage.DrugId == Guid.Empty)
            throw new BadRequestException("DrugUsage DrugId is required.");

        drugUsage.UpdatedBy = "system";

        _logger.LogDebug("Adding new drug usage for DrugId: {DrugId}.", drugUsage.DrugId);

        var result = await _repository.AddAsync(drugUsage, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added drug usage for DrugId: {DrugId}.", drugUsage.DrugId);

        return result;
    }

    public async Task<Common.DrugUsage.DrugUsage?> UpdateAsync(Common.DrugUsage.DrugUsage? drugUsage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drugUsage);

        if (drugUsage.DrugId == Guid.Empty)
            throw new BadRequestException("DrugUsage DrugId is required.");

        drugUsage.UpdatedBy = "system";

        _logger.LogDebug("Updating drug usage with id: {Id}.", drugUsage.Id);

        var result = await _repository.UpdateAsync(drugUsage, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated drug usage with id: {Id}.", drugUsage.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing drug usage with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed drug usage with id: {Id}.", id);
    }
}
