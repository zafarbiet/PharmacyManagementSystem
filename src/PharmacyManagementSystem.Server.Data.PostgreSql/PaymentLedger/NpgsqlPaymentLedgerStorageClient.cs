using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.PaymentLedger;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.PaymentLedger;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.PaymentLedger;

public class NpgsqlPaymentLedgerStorageClient(ILogger<NpgsqlPaymentLedgerStorageClient> logger, INpgsqlDbClient dbClient) : IPaymentLedgerStorageClient
{
    private readonly ILogger<NpgsqlPaymentLedgerStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.PaymentLedger.PaymentLedger?>?> GetByFilterCriteriaAsync(PaymentLedgerFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting paymentLedgers by filter criteria.");
        var sql = await PaymentLedgerDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PaymentLedger.PaymentLedger>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} paymentLedgers.", list.Count);
        return list;
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting paymentLedger by id: {Id}.", id);
        var sql = await PaymentLedgerDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PaymentLedger.PaymentLedger>(sql, cancellationToken).ConfigureAwait(false);
        var paymentLedger = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved paymentLedger with id: {Id}.", id);
        return paymentLedger;
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> AddAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);
        _logger.LogDebug("StorageClient: Adding paymentLedger.");
        var sql = await PaymentLedgerDatabaseCommandText.GetInsertSql(paymentLedger).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PaymentLedger.PaymentLedger>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added paymentLedger.");
        return inserted;
    }

    public async Task<Common.PaymentLedger.PaymentLedger?> UpdateAsync(Common.PaymentLedger.PaymentLedger? paymentLedger, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(paymentLedger);
        _logger.LogDebug("StorageClient: Updating paymentLedger with id: {Id}.", paymentLedger.Id);
        var sql = await PaymentLedgerDatabaseCommandText.GetUpdateSql(paymentLedger).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.PaymentLedger.PaymentLedger>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated paymentLedger with id: {Id}.", paymentLedger.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing paymentLedger with id: {Id}.", id);
        var sql = await PaymentLedgerDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed paymentLedger with id: {Id}.", id);
    }
}
