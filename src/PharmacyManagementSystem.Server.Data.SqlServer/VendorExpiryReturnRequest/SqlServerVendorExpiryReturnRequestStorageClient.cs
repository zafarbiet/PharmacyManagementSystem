using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.VendorExpiryReturnRequest;

namespace PharmacyManagementSystem.Server.Data.SqlServer.VendorExpiryReturnRequest;

public class SqlServerVendorExpiryReturnRequestStorageClient(ILogger<SqlServerVendorExpiryReturnRequestStorageClient> logger, ISqlServerDbClient dbClient) : IVendorExpiryReturnRequestStorageClient
{
    private readonly ILogger<SqlServerVendorExpiryReturnRequestStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>?> GetByFilterCriteriaAsync(VendorExpiryReturnRequestFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting vendor expiry return requests by filter criteria.");

        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} vendor expiry return requests.", list.Count);

        return list;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting vendor expiry return request by id: {Id}.", id);

        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(sql, cancellationToken).ConfigureAwait(false);

        var item = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved vendor expiry return request with id: {Id}.", id);

        return item;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> AddAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);

        _logger.LogDebug("StorageClient: Adding vendor expiry return request for expiry record: {ExpiryRecordId}.", vendorExpiryReturnRequest.ExpiryRecordId);

        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetInsertSql(vendorExpiryReturnRequest).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added vendor expiry return request for expiry record: {ExpiryRecordId}.", vendorExpiryReturnRequest.ExpiryRecordId);

        return inserted;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> UpdateAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);

        _logger.LogDebug("StorageClient: Updating vendor expiry return request with id: {Id}.", vendorExpiryReturnRequest.Id);

        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetUpdateSql(vendorExpiryReturnRequest).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated vendor expiry return request with id: {Id}.", vendorExpiryReturnRequest.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing vendor expiry return request with id: {Id}.", id);

        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed vendor expiry return request with id: {Id}.", id);
    }
}
