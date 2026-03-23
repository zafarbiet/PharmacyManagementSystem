using Dapper;
using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Report;
using PharmacyManagementSystem.Server.Data.SqlServer.Infrastructure;
using PharmacyManagementSystem.Server.Report;

namespace PharmacyManagementSystem.Server.Data.SqlServer.Report;

public class SqlServerReportService(ILogger<SqlServerReportService> logger, ISqlServerDbClient dbClient) : IReportService
{
    private readonly ILogger<SqlServerReportService> _logger = logger;
    private readonly ISqlServerDbClient _dbClient = dbClient;

    public async Task<DailySalesReport> GetDailySalesReportAsync(DateOnly date, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Generating daily sales report for {Date}.", date);

        var parameters = new DynamicParameters();
        parameters.Add("Date", date.ToDateTime(TimeOnly.MinValue));

        var sql = new DatabaseSqlWithParameters
        {
            SqlStatement = @"SELECT
                COUNT(*) AS InvoiceCount,
                ISNULL(SUM(SubTotal), 0) AS SubTotal,
                ISNULL(SUM(DiscountAmount), 0) AS TotalDiscount,
                ISNULL(SUM(TotalCgst), 0) AS TotalCgst,
                ISNULL(SUM(TotalSgst), 0) AS TotalSgst,
                ISNULL(SUM(TotalIgst), 0) AS TotalIgst,
                ISNULL(SUM(GstAmount), 0) AS GstAmount,
                ISNULL(SUM(NetAmount), 0) AS NetAmount
            FROM PMS.CustomerInvoices
            WHERE CAST(InvoiceDate AS DATE) = CAST(@Date AS DATE)
              AND IsActive = 1",
            Parameters = parameters
        };

        var rows = await _dbClient.QueryAsync<DailySalesReportRow>(sql, cancellationToken).ConfigureAwait(false);
        var row = rows.FirstOrDefault() ?? new DailySalesReportRow();

        _logger.LogDebug("Daily sales report for {Date}: {InvoiceCount} invoices, net ₹{NetAmount}.", date, row.InvoiceCount, row.NetAmount);

        return new DailySalesReport
        {
            Date = date,
            InvoiceCount = row.InvoiceCount,
            SubTotal = row.SubTotal,
            TotalDiscount = row.TotalDiscount,
            TotalCgst = row.TotalCgst,
            TotalSgst = row.TotalSgst,
            TotalIgst = row.TotalIgst,
            GstAmount = row.GstAmount,
            NetAmount = row.NetAmount
        };
    }

    private sealed class DailySalesReportRow
    {
        public int InvoiceCount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalCgst { get; set; }
        public decimal TotalSgst { get; set; }
        public decimal TotalIgst { get; set; }
        public decimal GstAmount { get; set; }
        public decimal NetAmount { get; set; }
    }
}
