using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DamageDisposalRecord;

namespace PharmacyManagementSystem.Server.DamageDisposalRecord;

public class GetDamageDisposalRecordAction(ILogger<GetDamageDisposalRecordAction> logger, IDamageDisposalRecordRepository repository) : IGetDamageDisposalRecordAction
{
    private readonly ILogger<GetDamageDisposalRecordAction> _logger = logger;
    private readonly IDamageDisposalRecordRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DamageDisposalRecord.DamageDisposalRecord>?> GetByFilterCriteriaAsync(DamageDisposalRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting damage disposal records by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} damage disposal records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DamageDisposalRecord.DamageDisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting damage disposal record by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved damage disposal record with id: {Id}.", id);

        return result;
    }
}
