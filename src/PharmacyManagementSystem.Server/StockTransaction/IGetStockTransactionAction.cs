using PharmacyManagementSystem.Common.StockTransaction;

namespace PharmacyManagementSystem.Server.StockTransaction;

public interface IGetStockTransactionAction
{
    Task<IReadOnlyCollection<Common.StockTransaction.StockTransaction>?> GetByFilterCriteriaAsync(StockTransactionFilter filter, CancellationToken cancellationToken);
    Task<Common.StockTransaction.StockTransaction?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
