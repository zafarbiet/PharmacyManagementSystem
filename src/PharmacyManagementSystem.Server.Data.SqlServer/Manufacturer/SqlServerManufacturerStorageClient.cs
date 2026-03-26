using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Manufacturer;
using PharmacyManagementSystem.Server.Manufacturer;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Manufacturer;

public class SqlServerManufacturerStorageClient(ILogger<SqlServerManufacturerStorageClient> logger, ISqlServerDbClient dbClient) : IManufacturerStorageClient
{
    private readonly ILogger<SqlServerManufacturerStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Manufacturer.Manufacturer>?> GetByFilterCriteriaAsync(ManufacturerFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting manufacturers by filter criteria.");

        var sql = await ManufacturerDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Manufacturer.Manufacturer>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} manufacturers.", list.Count);

        return list;
    }

    public async Task<Common.Manufacturer.Manufacturer?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting manufacturer by id: {Id}.", id);

        var sql = await ManufacturerDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Manufacturer.Manufacturer>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task<Common.Manufacturer.Manufacturer?> AddAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        _logger.LogDebug("StorageClient: Adding manufacturer with name: {Name}.", manufacturer.Name);

        var sql = await ManufacturerDatabaseCommandText.GetInsertSql(manufacturer).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Manufacturer.Manufacturer>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task<Common.Manufacturer.Manufacturer?> UpdateAsync(Common.Manufacturer.Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        _logger.LogDebug("StorageClient: Updating manufacturer with id: {Id}.", manufacturer.Id);

        var sql = await ManufacturerDatabaseCommandText.GetUpdateSql(manufacturer).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Manufacturer.Manufacturer>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing manufacturer with id: {Id}.", id);

        var sql = await ManufacturerDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
    }
}
