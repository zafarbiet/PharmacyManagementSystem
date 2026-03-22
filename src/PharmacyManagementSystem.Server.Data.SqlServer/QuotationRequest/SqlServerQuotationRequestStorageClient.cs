using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationRequest;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.QuotationRequest;

namespace PharmacyManagementSystem.Server.Data.SqlServer.QuotationRequest;

public class SqlServerQuotationRequestStorageClient(ILogger<SqlServerQuotationRequestStorageClient> logger, ISqlServerDbClient dbClient) : IQuotationRequestStorageClient
{
    private readonly ILogger<SqlServerQuotationRequestStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.QuotationRequest.QuotationRequest>?> GetByFilterCriteriaAsync(QuotationRequestFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting quotation requests by filter criteria.");

        var sql = await QuotationRequestDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequest.QuotationRequest>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} quotation requests.", list.Count);

        return list;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting quotation request by id: {Id}.", id);

        var sql = await QuotationRequestDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequest.QuotationRequest>(sql, cancellationToken).ConfigureAwait(false);

        var quotationRequest = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved quotation request with id: {Id}.", id);

        return quotationRequest;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> AddAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);

        _logger.LogDebug("StorageClient: Adding quotation request.");

        var sql = await QuotationRequestDatabaseCommandText.GetInsertSql(quotationRequest).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequest.QuotationRequest>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added quotation request.");

        return inserted;
    }

    public async Task<Common.QuotationRequest.QuotationRequest?> UpdateAsync(Common.QuotationRequest.QuotationRequest? quotationRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotationRequest);

        _logger.LogDebug("StorageClient: Updating quotation request with id: {Id}.", quotationRequest.Id);

        var sql = await QuotationRequestDatabaseCommandText.GetUpdateSql(quotationRequest).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationRequest.QuotationRequest>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated quotation request with id: {Id}.", quotationRequest.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing quotation request with id: {Id}.", id);

        var sql = await QuotationRequestDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed quotation request with id: {Id}.", id);
    }
}
