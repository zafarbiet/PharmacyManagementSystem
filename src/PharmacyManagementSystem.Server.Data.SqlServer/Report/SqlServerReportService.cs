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
                ISNULL(SUM(SubTotal), 0) AS SubTotal,
                ISNULL(SUM(DiscountAmount), 0) AS TotalDiscount,
                ISNULL(SUM(TotalCgst), 0) AS TotalCgst,
                ISNULL(SUM(TotalSgst), 0) AS TotalSgst,
                ISNULL(SUM(TotalIgst), 0) AS TotalIgst,
                ISNULL(SUM(GstAmount), 0) AS GstAmount,
                ISNULL(SUM(NetAmount), 0) AS NetAmount
            FROM PMS.CustomerInvoices
            WHERE YEAR(InvoiceDate) = @Year
              AND MONTH(InvoiceDate) = @Month
              AND IsActive = 1",
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
            FROM PMS.DrugInventory di
            INNER JOIN PMS.Drugs d ON d.Id = di.DrugId
            LEFT JOIN PMS.PurchaseOrderItems poi ON poi.DrugId = di.DrugId AND poi.IsActive = 1
            WHERE di.IsActive = 1 AND d.IsActive = 1
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

    public async Task<IReadOnlyList<ProfitMarginItem>> GetProfitMarginReportAsync(DateOnly dateFrom, DateOnly dateTo, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Generating profit margin report for {DateFrom} to {DateTo}.", dateFrom, dateTo);

        var parameters = new DynamicParameters();
        parameters.Add("DateFrom", dateFrom.ToDateTime(TimeOnly.MinValue));
        parameters.Add("DateTo", dateTo.ToDateTime(TimeOnly.MaxValue));

        var sql = new DatabaseSqlWithParameters
        {
            SqlStatement = @"SELECT
                d.Id                                                                          AS DrugId,
                d.Name                                                                        AS DrugName,
                cii.HsnCode,
                SUM(cii.Quantity)                                                             AS TotalQtySold,
                ISNULL(SUM(CAST(cii.Quantity AS DECIMAL(18,4)) * cii.UnitPrice), 0)          AS TotalRevenue,
                ISNULL(AVG(dp.CostPrice), 0)                                                 AS AverageCostPrice,
                ISNULL(SUM(CAST(cii.Quantity AS DECIMAL(18,4)) * ISNULL(dp.CostPrice, 0)), 0) AS TotalCost,
                ISNULL(SUM(CAST(cii.Quantity AS DECIMAL(18,4)) * cii.UnitPrice), 0)
                  - ISNULL(SUM(CAST(cii.Quantity AS DECIMAL(18,4)) * ISNULL(dp.CostPrice, 0)), 0) AS GrossProfit,
                ISNULL(d.Mrp, 0)                                                              AS Mrp,
                CASE WHEN ISNULL(SUM(CAST(cii.Quantity AS DECIMAL(18,4)) * cii.UnitPrice), 0) > 0
                     THEN (ISNULL(SUM(CAST(cii.Quantity AS DECIMAL(18,4)) * cii.UnitPrice), 0)
                           - ISNULL(SUM(CAST(cii.Quantity AS DECIMAL(18,4)) * ISNULL(dp.CostPrice, 0)), 0))
                          / SUM(CAST(cii.Quantity AS DECIMAL(18,4)) * cii.UnitPrice) * 100
                     ELSE 0 END                                                               AS MarginPct,
                CASE WHEN ISNULL(d.Mrp, 0) > 0 AND ISNULL(AVG(dp.CostPrice), 0) > 0
                     THEN (d.Mrp - AVG(dp.CostPrice)) / d.Mrp * 100
                     ELSE NULL END                                                            AS MrpMarginPct
            FROM PMS.CustomerInvoiceItems cii
            INNER JOIN PMS.CustomerInvoices ci ON ci.Id = cii.InvoiceId AND ci.IsActive = 1
            INNER JOIN PMS.Drugs d            ON d.Id = cii.DrugId AND d.IsActive = 1
            LEFT  JOIN PMS.DrugPricing dp     ON dp.DrugId = d.Id AND dp.IsActive = 1
            WHERE cii.IsActive = 1
              AND ci.InvoiceDate >= @DateFrom
              AND ci.InvoiceDate <= @DateTo
            GROUP BY d.Id, d.Name, cii.HsnCode, d.Mrp
            ORDER BY GrossProfit DESC",
            Parameters = parameters
        };

        var rows = await _dbClient.QueryAsync<ProfitMarginRow>(sql, cancellationToken).ConfigureAwait(false);

        return rows.Select(r => new ProfitMarginItem
        {
            DrugId = r.DrugId,
            DrugName = r.DrugName,
            HsnCode = r.HsnCode,
            TotalQtySold = r.TotalQtySold,
            TotalRevenue = r.TotalRevenue,
            AverageCostPrice = r.AverageCostPrice,
            TotalCost = r.TotalCost,
            GrossProfit = r.GrossProfit,
            Mrp = r.Mrp,
            MarginPct = r.MarginPct,
            MrpMarginPct = r.MrpMarginPct
        }).ToList();
    }

    private sealed class ProfitMarginRow
    {
        public Guid DrugId { get; set; }
        public string? DrugName { get; set; }
        public string? HsnCode { get; set; }
        public int TotalQtySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal? AverageCostPrice { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal Mrp { get; set; }
        public decimal MarginPct { get; set; }
        public decimal? MrpMarginPct { get; set; }
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
