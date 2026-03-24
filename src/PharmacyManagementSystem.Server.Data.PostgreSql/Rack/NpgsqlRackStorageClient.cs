using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Rack;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.Rack;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Rack;

public class NpgsqlRackStorageClient(ILogger<NpgsqlRackStorageClient> logger, INpgsqlDbClient dbClient) : IRackStorageClient
{
    private readonly ILogger<NpgsqlRackStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Rack.Rack?>?> GetByFilterCriteriaAsync(RackFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting racks by filter criteria.");
        var sql = await RackDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Rack.Rack>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} racks.", list.Count);
        return list;
    }

    public async Task<Common.Rack.Rack?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting rack by id: {Id}.", id);
        var sql = await RackDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Rack.Rack>(sql, cancellationToken).ConfigureAwait(false);
        var rack = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved rack with id: {Id}.", id);
        return rack;
    }

    public async Task<Common.Rack.Rack?> AddAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rack);
        _logger.LogDebug("StorageClient: Adding rack.");
        var sql = await RackDatabaseCommandText.GetInsertSql(rack).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Rack.Rack>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added rack.");
        return inserted;
    }

    public async Task<Common.Rack.Rack?> UpdateAsync(Common.Rack.Rack? rack, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rack);
        _logger.LogDebug("StorageClient: Updating rack with id: {Id}.", rack.Id);
        var sql = await RackDatabaseCommandText.GetUpdateSql(rack).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Rack.Rack>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated rack with id: {Id}.", rack.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing rack with id: {Id}.", id);
        var sql = await RackDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed rack with id: {Id}.", id);
    }
}
