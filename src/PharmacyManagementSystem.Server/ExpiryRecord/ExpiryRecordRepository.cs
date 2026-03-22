using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.ExpiryRecord;

namespace PharmacyManagementSystem.Server.ExpiryRecord;

public class ExpiryRecordRepository(ILogger<ExpiryRecordRepository> logger, IExpiryRecordStorageClient storageClient) : IExpiryRecordRepository
{
    private readonly ILogger<ExpiryRecordRepository> _logger = logger;
    private readonly IExpiryRecordStorageClient _storageClient = storageClient;

    public async Task<IReadOnlyCollection<Common.ExpiryRecord.ExpiryRecord>?> GetByFilterCriteriaAsync(ExpiryRecordFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("Repository: Getting expiry records by filter criteria.");

        var result = await _storageClient.GetByFilterCriteriaAsync(filter, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved {Count} expiry records.", result?.Count ?? 0);

        return result;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("Repository: Getting expiry record by id: {Id}.", id);

        var result = await _storageClient.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Retrieved expiry record with id: {Id}.", id);

        return result;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> AddAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);

        _logger.LogDebug("Repository: Adding expiry record for drug inventory: {DrugInventoryId}.", expiryRecord.DrugInventoryId);

        var result = await _storageClient.AddAsync(expiryRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Added expiry record for drug inventory: {DrugInventoryId}.", expiryRecord.DrugInventoryId);

        return result;
    }

    public async Task<Common.ExpiryRecord.ExpiryRecord?> UpdateAsync(Common.ExpiryRecord.ExpiryRecord? expiryRecord, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(expiryRecord);

        _logger.LogDebug("Repository: Updating expiry record with id: {Id}.", expiryRecord.Id);

        var result = await _storageClient.UpdateAsync(expiryRecord, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Updated expiry record with id: {Id}.", expiryRecord.Id);

        return result;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("Repository: Removing expiry record with id: {Id}.", id);

        await _storageClient.RemoveAsync(id, updatedBy, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Repository: Removed expiry record with id: {Id}.", id);
    }
}
