using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.DebtPayment;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.DebtPayment;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.DebtPayment;

public class NpgsqlDebtPaymentStorageClient(ILogger<NpgsqlDebtPaymentStorageClient> logger, INpgsqlDbClient dbClient) : IDebtPaymentStorageClient
{
    private readonly ILogger<NpgsqlDebtPaymentStorageClient> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<IReadOnlyCollection<Common.DebtPayment.DebtPayment?>?> GetByFilterCriteriaAsync(DebtPaymentFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);
        _logger.LogDebug("StorageClient: Getting debtPayments by filter criteria.");
        var sql = await DebtPaymentDatabaseCommandText.GetSelectSql(filter).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtPayment.DebtPayment>(sql, cancellationToken).ConfigureAwait(false);
        var list = result.ToList().AsReadOnly();
        _logger.LogDebug("StorageClient: Retrieved {Count} debtPayments.", list.Count);
        return list;
    }

    public async Task<Common.DebtPayment.DebtPayment?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        _logger.LogDebug("StorageClient: Getting debtPayment by id: {Id}.", id);
        var sql = await DebtPaymentDatabaseCommandText.GetSelectByIdSql(id).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtPayment.DebtPayment>(sql, cancellationToken).ConfigureAwait(false);
        var debtPayment = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Retrieved debtPayment with id: {Id}.", id);
        return debtPayment;
    }

    public async Task<Common.DebtPayment.DebtPayment?> AddAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtPayment);
        _logger.LogDebug("StorageClient: Adding debtPayment.");
        var sql = await DebtPaymentDatabaseCommandText.GetInsertSql(debtPayment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtPayment.DebtPayment>(sql, cancellationToken).ConfigureAwait(false);
        var inserted = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Added debtPayment.");
        return inserted;
    }

    public async Task<Common.DebtPayment.DebtPayment?> UpdateAsync(Common.DebtPayment.DebtPayment? debtPayment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(debtPayment);
        _logger.LogDebug("StorageClient: Updating debtPayment with id: {Id}.", debtPayment.Id);
        var sql = await DebtPaymentDatabaseCommandText.GetUpdateSql(debtPayment).ConfigureAwait(false);
        var result = await _dbClient.QueryAsync<Common.DebtPayment.DebtPayment>(sql, cancellationToken).ConfigureAwait(false);
        var updated = result.FirstOrDefault();
        _logger.LogDebug("StorageClient: Updated debtPayment with id: {Id}.", debtPayment.Id);
        return updated;
    }

    public async Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updatedBy);
        _logger.LogDebug("StorageClient: Removing debtPayment with id: {Id}.", id);
        var sql = await DebtPaymentDatabaseCommandText.GetSoftDeleteSql(id, updatedBy).ConfigureAwait(false);
        await _dbClient.ExecuteAsync(sql, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("StorageClient: Removed debtPayment with id: {Id}.", id);
    }
}
