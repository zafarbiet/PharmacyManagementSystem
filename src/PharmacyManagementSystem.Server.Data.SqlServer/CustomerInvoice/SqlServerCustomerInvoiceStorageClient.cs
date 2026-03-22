using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerInvoice;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.CustomerInvoice;

namespace PharmacyManagementSystem.Server.Data.SqlServer.CustomerInvoice;

public class SqlServerCustomerInvoiceStorageClient(ILogger<SqlServerCustomerInvoiceStorageClient> logger, ISqlServerDbClient dbClient) : ICustomerInvoiceStorageClient
{
    private readonly ILogger<SqlServerCustomerInvoiceStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.CustomerInvoice.CustomerInvoice>?> GetByFilterCriteriaAsync(CustomerInvoiceFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting customer invoices by filter criteria.");

        var sql = await CustomerInvoiceDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerInvoice.CustomerInvoice>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} customer invoices.", list.Count);

        return list;
    }

    public async Task<Common.CustomerInvoice.CustomerInvoice?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting customer invoice by id: {Id}.", id);

        var sql = await CustomerInvoiceDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerInvoice.CustomerInvoice>(sql, cancellationToken).ConfigureAwait(false);

        var customerInvoice = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved customer invoice with id: {Id}.", id);

        return customerInvoice;
    }

    public async Task<Common.CustomerInvoice.CustomerInvoice?> AddAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        _logger.LogDebug("StorageClient: Adding customer invoice with status: {Status}.", customerInvoice.Status);

        var sql = await CustomerInvoiceDatabaseCommandText.GetInsertSql(customerInvoice).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerInvoice.CustomerInvoice>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added customer invoice with status: {Status}.", customerInvoice.Status);

        return inserted;
    }

    public async Task<Common.CustomerInvoice.CustomerInvoice?> UpdateAsync(Common.CustomerInvoice.CustomerInvoice? customerInvoice, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoice);

        _logger.LogDebug("StorageClient: Updating customer invoice with id: {Id}.", customerInvoice.Id);

        var sql = await CustomerInvoiceDatabaseCommandText.GetUpdateSql(customerInvoice).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerInvoice.CustomerInvoice>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated customer invoice with id: {Id}.", customerInvoice.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing customer invoice with id: {Id}.", id);

        var sql = await CustomerInvoiceDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed customer invoice with id: {Id}.", id);
    }
}
