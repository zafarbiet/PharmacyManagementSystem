using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.VendorExpiryReturnRequest;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.VendorExpiryReturnRequest;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.VendorExpiryReturnRequest;

public class NpgsqlVendorExpiryReturnRequestStorageClient(ILogger<NpgsqlVendorExpiryReturnRequestStorageClient> logger, INpgsqlDbClient dbClient) : IVendorExpiryReturnRequestStorageClient
{
    private readonly ILogger<NpgsqlVendorExpiryReturnRequestStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?>?> GetByFilterCriteriaAsync(VendorExpiryReturnRequestFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting vendorExpiryReturnRequests by filter criteria.");
        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} vendorExpiryReturnRequests.", list.Count);
        return list;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting vendorExpiryReturnRequest by id: {Id}.", id);
        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(sql, cancellationToken).ConfigureAwait(false);
        var vendorExpiryReturnRequest = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved vendorExpiryReturnRequest with id: {Id}.", id);
        return vendorExpiryReturnRequest;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> AddAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);
        _logger.LogDebug("StorageClient: Adding vendorExpiryReturnRequest.");
        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetInsertSql(vendorExpiryReturnRequest).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added vendorExpiryReturnRequest.");
        return inserted;
    }

    public async Task<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest?> UpdateAsync(Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest? vendorExpiryReturnRequest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendorExpiryReturnRequest);
        _logger.LogDebug("StorageClient: Updating vendorExpiryReturnRequest with id: {Id}.", vendorExpiryReturnRequest.Id);
        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetUpdateSql(vendorExpiryReturnRequest).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.VendorExpiryReturnRequest.VendorExpiryReturnRequest>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated vendorExpiryReturnRequest with id: {Id}.", vendorExpiryReturnRequest.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing vendorExpiryReturnRequest with id: {Id}.", id);
        var sql = await VendorExpiryReturnRequestDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed vendorExpiryReturnRequest with id: {Id}.", id);
    }
}
