using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DrugInventoryRackAssignment;

namespace PharmacyManagementSystem.Server.DrugInventoryRackAssignment;

public class GetDrugInventoryRackAssignmentAction(ILogger<GetDrugInventoryRackAssignmentAction> logger, IDrugInventoryRackAssignmentRepository repository) : IGetDrugInventoryRackAssignmentAction
{
    private readonly ILogger<GetDrugInventoryRackAssignmentAction> _logger = logger;
    private readonly IDrugInventoryRackAssignmentRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment>?> GetByFilterCriteriaAsync(DrugInventoryRackAssignmentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting drug inventory rack assignments by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} drug inventory rack assignments.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DrugInventoryRackAssignment.DrugInventoryRackAssignment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting drug inventory rack assignment by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved drug inventory rack assignment with id: {Id}.", id);

        return result;
    }
}
