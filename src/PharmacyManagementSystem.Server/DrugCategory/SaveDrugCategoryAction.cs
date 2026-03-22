using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Exceptions;

namespace PharmacyManagementSystem.Server.DrugCategory;

public class SaveDrugCategoryAction(ILogger<SaveDrugCategoryAction> logger, IDrugCategoryRepository repository) : ISaveDrugCategoryAction
{
    private readonly ILogger<SaveDrugCategoryAction> _logger = logger;
    private readonly IDrugCategoryRepository _repository = repository;

    public async Task<Common.DrugCategory.DrugCategory?> AddAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (string.IsNullOrWhiteSpace(category.Name))
            throw new BadRequestException("Drug category Name is required.");

        category.UpdatedBy = "system";

        _logger.LogDebug("Adding new drug category with name: {Name}.", category.Name);

        var result = await _repository.AddAsync(category, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Added drug category with name: {Name}.", category.Name);

        return result;
    }

    public async Task<Common.DrugCategory.DrugCategory?> UpdateAsync(Common.DrugCategory.DrugCategory? category, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (string.IsNullOrWhiteSpace(category.Name))
            throw new BadRequestException("Drug category Name is required.");

        category.UpdatedBy = "system";

        _logger.LogDebug("Updating drug category with id: {Id}.", category.Id);

        var result = await _repository.UpdateAsync(category, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Updated drug category with id: {Id}.", category.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Removing drug category with id: {Id}.", id);

        await _repository.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Removed drug category with id: {Id}.", id);
    }
}
