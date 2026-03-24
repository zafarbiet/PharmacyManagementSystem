using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationRequest;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.QuotationRequest;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.QuotationRequest;

public class NpgsqlQuotationRequestStorageClient(ILogger<NpgsqlQuotationRequestStorageClient> logger, INpgsqlDbClient dbClient) : IQuotationRequestStorageClient
{
    private readonly ILogger<NpgsqlQuotationRequestStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.QuotationRequest.QuotationRequest?>?> GetByFilterCriteriaAsync(QuotationRequestFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting quotationRequests by filter criteria.");
        var sql = await QuotationRequestDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequest.QuotationRequest>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} quotationRequests.", list.Count);
        return list;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting quotationRequest by id: {Id}.", id);
        var sql = await QuotationRequestDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequest.QuotationRequest>(sql, cancellationToken).ConfigureAwait(false);
        var quotationRequest = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved quotationRequest with id: {Id}.", id);
        return quotationRequest;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> AddAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);
        _logger.LogDebug("StorageClient: Adding quotationRequest.");
        var sql = await QuotationRequestDatabaseCommandText.GetInsertSql(quotationRequest).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequest.QuotationRequest>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added quotationRequest.");
        return inserted;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> UpdateAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);
        _logger.LogDebug("StorageClient: Updating quotationRequest with id: {Id}.", quotationRequest.Id);
        var sql = await QuotationRequestDatabaseCommandText.GetUpdateSql(quotationRequest).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequest.QuotationRequest>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated quotationRequest with id: {Id}.", quotationRequest.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing quotationRequest with id: {Id}.", id);
        var sql = await QuotationRequestDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed quotationRequest with id: {Id}.", id);
    }
}
