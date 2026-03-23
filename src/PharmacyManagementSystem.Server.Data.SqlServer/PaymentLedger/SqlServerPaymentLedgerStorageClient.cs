using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PaymentLedger;
using PharmacyManagementSystem.Server.PaymentLedger;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;

namespace PharmacyManagementSystem.Server.Data.SqlServer.PaymentLedger;

public class SqlServerPaymentLedgerStorageClient(ILogger<SqlServerPaymentLedgerStorageClient> logger, ISqlServerDbClient dbClient) : IPaymentLedgerStorageClient
{
    private readonly ILogger<SqlServerPaymentLedgerStorageClient> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.PaymentLedger.PaymentLedger>?> GetByFilterCriteriaAsync(PaymentLedgerFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        _logger.LogDebug("StorageClient: Getting payment ledgers by filter criteria.");

        var sql = await PaymentLedgerDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PaymentLedger.PaymentLedger>(sql, cancellationToken).ConfigureAwait(false);

        var list = result.ToList().AsReadOnly();

        _logger.LogDebug("StorageClient: Retrieved {Count} payment ledgers.", list.Count);

        return list;
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _logger.LogDebug("StorageClient: Getting payment ledger by id: {Id}.", id);

        var sql = await PaymentLedgerDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PaymentLedger.PaymentLedger>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> AddAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);

        _logger.LogDebug("StorageClient: Adding payment ledger for vendor: {VendorId}.", paymentLedger.VendorId);

        var sql = await PaymentLedgerDatabaseCommandText.GetInsertSql(paymentLedger).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PaymentLedger.PaymentLedger>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> UpdateAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);

        _logger.LogDebug("StorageClient: Updating payment ledger with id: {Id}.", paymentLedger.Id);

        var sql = await PaymentLedgerDatabaseCommandText.GetUpdateSql(paymentLedger).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PaymentLedger.PaymentLedger>(sql, cancellationToken).ConfigureAwait(false);

        return result.FirstOrDefault();
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);

        _logger.LogDebug("StorageClient: Removing payment ledger with id: {Id}.", id);

        var sql = await PaymentLedgerDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
    }
}
