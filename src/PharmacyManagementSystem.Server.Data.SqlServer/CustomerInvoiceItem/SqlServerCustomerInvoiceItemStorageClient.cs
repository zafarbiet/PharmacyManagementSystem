using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.CustomerInvoiceItem;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.CustomerInvoiceItem;

namespace PharmacyManagementSystem.Server.Data.SqlServer.CustomerInvoiceItem;

public class SqlServerCustomerInvoiceItemStorageClient(ILogger<SqlServerCustomerInvoiceItemStorageClient> logger, ISqlServerDbClient dbClient) : ICustomerInvoiceItemStorageClient
{
    private readonly ILogger<SqlServerCustomerInvoiceItemStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.CustomerInvoiceItem.CustomerInvoiceItem>?> GetByFilterCriteriaAsync(CustomerInvoiceItemFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting customer invoice items by filter criteria.");

        var sql = await CustomerInvoiceItemDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerInvoiceItem.CustomerInvoiceItem>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} customer invoice items.", list.Count);

        return list;
    }

    public async Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting customer invoice item by id: {Id}.", id);

        var sql = await CustomerInvoiceItemDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerInvoiceItem.CustomerInvoiceItem>(sql, cancellationToken).ConfigureAwait(false);

        var customerInvoiceItem = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Retrieved customer invoice item with id: {Id}.", id);

        return customerInvoiceItem;
    }

    public async Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> AddAsync(Common.CustomerInvoiceItem.CustomerInvoiceItem? customerInvoiceItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoiceItem);

        _logger.LogDebug("StorageClient: Adding customer invoice item for InvoiceId: {InvoiceId}.", customerInvoiceItem.InvoiceId);

        var sql = await CustomerInvoiceItemDatabaseCommandText.GetInsertSql(customerInvoiceItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerInvoiceItem.CustomerInvoiceItem>(sql, cancellationToken).ConfigureAwait(false);

        var inserted = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Added customer invoice item for InvoiceId: {InvoiceId}.", customerInvoiceItem.InvoiceId);

        return inserted;
    }

    public async Task<Common.CustomerInvoiceItem.CustomerInvoiceItem?> UpdateAsync(Common.CustomerInvoiceItem.CustomerInvoiceItem? customerInvoiceItem, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customerInvoiceItem);

        _logger.LogDebug("StorageClient: Updating customer invoice item with id: {Id}.", customerInvoiceItem.Id);

        var sql = await CustomerInvoiceItemDatabaseCommandText.GetUpdateSql(customerInvoiceItem).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.CustomerInvoiceItem.CustomerInvoiceItem>(sql, cancellationToken).ConfigureAwait(false);

        var updated = result.FirstOrDefault();

        _logger.LogDebug("StorageClient: Updated customer invoice item with id: {Id}.", customerInvoiceItem.Id);

        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing customer invoice item with id: {Id}.", id);

        var sql = await CustomerInvoiceItemDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("StorageClient: Removed customer invoice item with id: {Id}.", id);
    }
}
