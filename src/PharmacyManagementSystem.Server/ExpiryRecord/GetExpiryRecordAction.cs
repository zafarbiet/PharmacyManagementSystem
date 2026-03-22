using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.ExpiryRecord;

namespace PharmacyManagementSystem.Server.ExpiryRecord;

public class GetExpiryRecordAction(ILogger<GetExpiryRecordAction> logger, IExpiryRecordRepository repository) : IGetExpiryRecordAction
{
    private readonly ILogger<GetExpiryRecordAction> _logger = logger;
    private readonly IExpiryRecordRepository _repository = repository;

    public async Task<IReadOnlyCollection<Common.ExpiryRecord.ExpiryRecord>?> GetByFilterCriteriaAsync(ExpiryRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Getting expiry records by filter criteria.");

        var result = await _repository.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved {Count} expiry records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Getting expiry record by id: {Id}.", id);

        var result = await _repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Retrieved expiry record with id: {Id}.", id);

        return result;
    }
}
