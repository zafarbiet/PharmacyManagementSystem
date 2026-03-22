using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Vendor;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Vendor;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Vendor;

public class SqlServerVendorStorageClient(ILogger<SqlServerVendorStorageClient> logger, ISqlServerDbClient dbClient) : IVendorStorageClient
{
    private readonly ILogger<SqlServerVendorStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Vendor.Vendor>?> GetByFilterCriteriaAsync(VendorFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting vendors by filter criteria.");

        var sql = await VendorDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Vendor.Vendor>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} vendors.", list.Count);

        return list;
    }

    public async Task<Common.Vendor.Vendor?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting vendor by id: {Id}.", id);

        var sql = await VendorDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Vendor.Vendor>(sql, cancellationToken).ConfigureAwait(false);

        var vendor = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved vendor with id: {Id}.", id);

        return vendor;
    }

    public async Task<Common.Vendor.Vendor?> AddAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        _logger.LogDebug("StorageClient: Adding vendor with name: {Name}.", vendor.Name);

        var sql = await VendorDatabaseCommandText.GetInsertSql(vendor).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Vendor.Vendor>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added vendor with name: {Name}.", vendor.Name);

        return inserted;
    }

    public async Task<Common.Vendor.Vendor?> UpdateAsync(Common.Vendor.Vendor? vendor, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        _logger.LogDebug("StorageClient: Updating vendor with id: {Id}.", vendor.Id);

        var sql = await VendorDatabaseCommandText.GetUpdateSql(vendor).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Vendor.Vendor>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated vendor with id: {Id}.", vendor.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing vendor with id: {Id}.", id);

        var sql = await VendorDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed vendor with id: {Id}.", id);
    }
}
