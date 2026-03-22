using PharmacyManagementSystem.Common.StockTransaction;

namespace PharmacyManagementSystem.Server.StockTransaction;

public interface IStockTransactionStorageClient
{
    Task<IReadOnlyCollection<Common.StockTransaction.StockTransaction>?> GetByFilterCriteriaAsync(StockTransactionFilter filter, CancellationToken cancellationToken);
    Task<Common.StockTransaction.StockTransaction?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Common.StockTransaction.StockTransaction?> AddAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken);
    Task<Common.StockTransaction.StockTransaction?> UpdateAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
