using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DebtRecord;

namespace PharmacyManagementSystem.Server.DebtRecord;

public class GetDebtRecordAction(ILogger<GetDebtRecordAction> logger, IDebtRecordRepository repository) : IGetDebtRecordAction
{
    private readonly ILogger<GetDebtRecordAction> _logger = logger;
    private readonly IDebtRecordRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DebtRecord.DebtRecord>?> GetByFilterCriteriaAsync(DebtRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting debt records by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} debt records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DebtRecord.DebtRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting debt record by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved debt record with id: {Id}.", id);

        return result;
    }
}
