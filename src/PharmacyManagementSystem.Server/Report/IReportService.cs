using PharmacyManagementSystem.Common.Report;

namespace PharmacyManagementSystem.Server.Report;

public interface IReportService
{
    Task<DailySalesReport> GetDailySalesReportAsync(DateOnly date, CancellationToken cancellationToken);
    Task<MonthlySalesReport> GetMonthlySalesReportAsync(int year, int month, CancellationToken cancellationToken);
    Task<IReadOnlyList<StockValuationItem>> GetStockValuationAsync(CancellationToken cancellationToken);
}
