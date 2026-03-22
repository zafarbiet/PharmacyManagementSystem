namespace PharmacyManagementSystem.Server.StockTransaction;

public interface ISaveStockTransactionAction
{
    Task<Common.StockTransaction.StockTransaction?> AddAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken);
    Task<Common.StockTransaction.StockTransaction?> UpdateAsync(Common.StockTransaction.StockTransaction? stockTransaction, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, string updatedBy, CancellationToken cancellationToken);
}
