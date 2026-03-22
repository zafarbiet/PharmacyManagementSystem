using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.Drug;

public class SaveDrugAction(ILogger<SaveDrugAction> logger, IDrugRepository repository) : ISaveDrugAction
{
    private readonly ILogger<SaveDrugAction> _logger = logger;
    private readonly IDrugRepository _repository = repository;

    public async Task<Common.Drug.Drug?> AddAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drug);

        if (string.IsNullOrWhiteSpace(drug.Name))
            throw new BadRequestException("Drug Name is required.");

        if (drug.CategoryId == Guid.Empty)
            throw new BadRequestException("Drug CategoryId is required.");

        drug.UpdatedBy = "system";

        _logger.LogDebug("Adding new drug with name: {Name}.", drug.Name);

        var result = await _repository.AddAsync(drug, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added drug with name: {Name}.", drug.Name);

        return result;
    }

    public async Task<Common.Drug.Drug?> UpdateAsync(Common.Drug.Drug? drug, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(drug);

        if (string.IsNullOrWhiteSpace(drug.Name))
            throw new BadRequestException("Drug Name is required.");

        if (drug.CategoryId == Guid.Empty)
            throw new BadRequestException("Drug CategoryId is required.");

        drug.UpdatedBy = "system";

        _logger.LogDebug("Updating drug with id: {Id}.", drug.Id);

        var result = await _repository.UpdateAsync(drug, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated drug with id: {Id}.", drug.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing drug with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed drug with id: {Id}.", id);
    }
}
