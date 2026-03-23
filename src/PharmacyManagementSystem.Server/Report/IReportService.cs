using PharmacyManagementSystem.Common.Report;

namespace PharmacyManagementSystem.Server.Report;

public interface IReportService
{
    Task<DailySalesReport> GetDailySalesReportAsync(DateOnly date, CancellationToken cancellationToken);
}
