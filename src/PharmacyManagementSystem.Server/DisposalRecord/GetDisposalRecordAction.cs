using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DisposalRecord;

namespace PharmacyManagementSystem.Server.DisposalRecord;

public class GetDisposalRecordAction(ILogger<GetDisposalRecordAction> logger, IDisposalRecordRepository repository) : IGetDisposalRecordAction
{
    private readonly ILogger<GetDisposalRecordAction> _logger = logger;
    private readonly IDisposalRecordRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.DisposalRecord.DisposalRecord>?> GetByFilterCriteriaAsync(DisposalRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting disposal records by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} disposal records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.DisposalRecord.DisposalRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting disposal record by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved disposal record with id: {Id}.", id);

        return result;
    }
}
