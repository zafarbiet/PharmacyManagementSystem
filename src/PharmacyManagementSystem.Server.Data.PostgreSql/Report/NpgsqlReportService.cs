using Dapper;
using Microsoft.Extensions.Logging;
using PharmacyManagementSystem.Common.Report;
using PharmacyManagementSystem.Server.Data.PostgreSql.Infrastructure;
using PharmacyManagementSystem.Server.Report;

namespace PharmacyManagementSystem.Server.Data.PostgreSql.Report;

public class NpgsqlReportService(ILogger<NpgsqlReportService> logger, INpgsqlDbClient dbClient) : IReportService
{
    private readonly ILogger<NpgsqlReportService> _logger = logger;
    private readonly INpgsqlDbClient _dbClient = dbClient;

    public async Task<DailySalesReport> GetDailySalesReportAsync(DateOnly date, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Generating daily sales report for {Date}.", date);

        var parameters = new DynamicParameters();
        parameters.Add("Date", date.ToDateTime(TimeOnly.MinValue));

        var sql = new DatabaseSqlWithParameters
        {
            SqlStatement = @"SELECT
                COUNT(*) AS InvoiceCount,
                COALESCE(SUM(SubTotal), 0) AS SubTotal,
                COALESCE(SUM(DiscountAmount), 0) AS TotalDiscount,
                COALESCE(SUM(TotalCgst), 0) AS TotalCgst,
                COALESCE(SUM(TotalSgst), 0) AS TotalSgst,
                COALESCE(SUM(TotalIgst), 0) AS TotalIgst,
                COALESCE(SUM(GstAmount), 0) AS GstAmount,
                COALESCE(SUM(NetAmount), 0) AS NetAmount
            FROM PMS.CustomerInvoices
            WHERE InvoiceDate::date = @Date::date
              AND IsActive = true",
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

    public async Task<MonthlySalesReport> GetMonthlySalesReportAsync(int year, int month, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Generating monthly sales report for {Year}-{Month}.", year, month);

        var parameters = new DynamicParameters();
        parameters.Add("Year", year);
        parameters.Add("Month", month);

        var sql = new DatabaseSqlWithParameters
        {
            SqlStatement = @"SELECT
                COUNT(*) AS InvoiceCount,
                COALESCE(SUM(SubTotal), 0) AS SubTotal,
                COALESCE(SUM(DiscountAmount), 0) AS TotalDiscount,
                COALESCE(SUM(TotalCgst), 0) AS TotalCgst,
                COALESCE(SUM(TotalSgst), 0) AS TotalSgst,
                COALESCE(SUM(TotalIgst), 0) AS TotalIgst,
                COALESCE(SUM(GstAmount), 0) AS GstAmount,
                COALESCE(SUM(NetAmount), 0) AS NetAmount
            FROM PMS.CustomerInvoices
            WHERE EXTRACT(YEAR FROM InvoiceDate)::int = @Year
              AND EXTRACT(MONTH FROM InvoiceDate)::int = @Month
              AND IsActive = true",
            Parameters = parameters
        };

        var rows = await _dbClient.QueryAsync<DailySalesReportRow>(sql, cancellationToken).ConfigureAwait(false);
        var row = rows.FirstOrDefault() ?? new DailySalesReportRow();

        return new MonthlySalesReport
        {
            Year = year,
            Month = month,
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

    public async Task<IReadOnlyList<StockValuationItem>> GetStockValuationAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Generating stock valuation report.");

        var sql = new DatabaseSqlWithParameters
        {
            SqlStatement = @"SELECT
                di.DrugId,
                d.Name AS DrugName,
                d.HsnCode,
                SUM(di.QuantityInStock) AS TotalQuantity,
                d.Mrp,
                SUM(di.QuantityInStock) * d.Mrp AS TotalMrpValue,
                AVG(poi.UnitPrice) AS AverageCostPrice,
                SUM(di.QuantityInStock) * AVG(poi.UnitPrice) AS TotalCostValue
            FROM PMS.DrugInventories di
            INNER JOIN PMS.Drugs d ON d.Id = di.DrugId
            LEFT JOIN PMS.PurchaseOrderItems poi ON poi.DrugId = di.DrugId AND poi.IsActive = true
            WHERE di.IsActive = true AND d.IsActive = true
            GROUP BY di.DrugId, d.Name, d.HsnCode, d.Mrp
            ORDER BY TotalMrpValue DESC",
            Parameters = new DynamicParameters()
        };

        var rows = await _dbClient.QueryAsync<StockValuationRow>(sql, cancellationToken).ConfigureAwait(false);

        return rows.Select(r => new StockValuationItem
        {
            DrugId = r.DrugId,
            DrugName = r.DrugName,
            HsnCode = r.HsnCode,
            TotalQuantity = r.TotalQuantity,
            Mrp = r.Mrp,
            TotalMrpValue = r.TotalMrpValue,
            AverageCostPrice = r.AverageCostPrice,
            TotalCostValue = r.TotalCostValue
        }).ToList();
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

    private sealed class StockValuationRow
    {
        public Guid DrugId { get; set; }
        public string? DrugName { get; set; }
        public string? HsnCode { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Mrp { get; set; }
        public decimal TotalMrpValue { get; set; }
        public decimal? AverageCostPrice { get; set; }
        public decimal? TotalCostValue { get; set; }
    }
}
