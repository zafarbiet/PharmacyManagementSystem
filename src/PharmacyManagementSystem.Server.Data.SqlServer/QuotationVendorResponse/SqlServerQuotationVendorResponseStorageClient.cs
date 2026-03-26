using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.QuotationVendorResponse;
using PharmacyManagementSystem.Server.QuotationVendorResponse;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.QuotationVendorResponse;

public class SqlServerQuotationVendorResponseStorageClient(ILogger<SqlServerQuotationVendorResponseStorageClient> logger, ISqlServerDbClient dbClient) : IQuotationVendorResponseStorageClient
{
    private readonly ILogger<SqlServerQuotationVendorResponseStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.QuotationVendorResponse.QuotationVendorResponse>?> GetByFilterCriteriaAsync(QuotationVendorResponseFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting quotation vendor responses by filter criteria.");

        var sql = await QuotationVendorResponseDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationVendorResponse.QuotationVendorResponse>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} quotation vendor responses.", list.Count);

        return list;
    }

    public async Task<Common.QuotationVendorResponse.QuotationVendorResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting quotation vendor response by id: {Id}.", id);

        var sql = await QuotationVendorResponseDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationVendorResponse.QuotationVendorResponse>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task<Common.QuotationVendorResponse.QuotationVendorResponse?> AddAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response);

        _logger.LogDebug("StorageClient: Adding quotation vendor response.");

        var sql = await QuotationVendorResponseDatabaseCommandText.GetInsertSql(response).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationVendorResponse.QuotationVendorResponse>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task<Common.QuotationVendorResponse.QuotationVendorResponse?> UpdateAsync(Common.QuotationVendorResponse.QuotationVendorResponse response, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response);

        _logger.LogDebug("StorageClient: Updating quotation vendor response with id: {Id}.", response.Id);

        var sql = await QuotationVendorResponseDatabaseCommandText.GetUpdateSql(response).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.QuotationVendorResponse.QuotationVendorResponse>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing quotation vendor response with id: {Id}.", id);

        var sql = await QuotationVendorResponseDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
    }
}
