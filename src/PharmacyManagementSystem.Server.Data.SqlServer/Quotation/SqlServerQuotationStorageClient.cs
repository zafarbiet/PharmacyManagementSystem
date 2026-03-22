using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Quotation;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Quotation;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Quotation;

public class SqlServerQuotationStorageClient(ILogger<SqlServerQuotationStorageClient> logger, ISqlServerDbClient dbClient) : IQuotationStorageClient
{
    private readonly ILogger<SqlServerQuotationStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.Quotation.Quotation>?> GetByFilterCriteriaAsync(QuotationFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting quotations by filter criteria.");

        var sql = await QuotationDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Quotation.Quotation>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} quotations.", list.Count);

        return list;
    }

    public async Task<Common.Quotation.Quotation?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting quotation by id: {Id}.", id);

        var sql = await QuotationDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Quotation.Quotation>(sql, cancellationToken).ConfigureAwait(false);

        var quotation = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved quotation with id: {Id}.", id);

        return quotation;
    }

    public async Task<Common.Quotation.Quotation?> AddAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        _logger.LogDebug("StorageClient: Adding quotation.");

        var sql = await QuotationDatabaseCommandText.GetInsertSql(quotation).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Quotation.Quotation>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added quotation.");

        return inserted;
    }

    public async Task<Common.Quotation.Quotation?> UpdateAsync(Common.Quotation.Quotation? quotation, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(quotation);

        _logger.LogDebug("StorageClient: Updating quotation with id: {Id}.", quotation.Id);

        var sql = await QuotationDatabaseCommandText.GetUpdateSql(quotation).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.Quotation.Quotation>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated quotation with id: {Id}.", quotation.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing quotation with id: {Id}.", id);

        var sql = await QuotationDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed quotation with id: {Id}.", id);
    }
}
