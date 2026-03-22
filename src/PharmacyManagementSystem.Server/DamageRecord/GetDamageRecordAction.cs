using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DamageRecord;

namespace PharmacyManagementSystem.Server.DamageRecord;

public class GetDamageRecordAction(ILogger<GetDamageRecordAction> logger, IDamageRecordRepository repository) : IGetDamageRecordAction
{
    private readonly ILogger<GetDamageRecordAction> _logger = logger;
    private readonly IDamageRecordRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DamageRecord.DamageRecord>?> GetByFilterCriteriaAsync(DamageRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting damage records by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} damage records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DamageRecord.DamageRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting damage record by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved damage record with id: {Id}.", id);

        return result;
    }
}
