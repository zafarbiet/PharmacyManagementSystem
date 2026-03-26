import { useState } from 'react';
import {
  DatePicker,
  Button,
  Typography,
  Row,
  Col,
  Card,
  Statistic,
  Divider,
  Alert,
  Spin,
  Descriptions,
  Tabs,
  Table,
  Tag,
} from 'antd';
import { SearchOutlined, BarChartOutlined, ReloadOutlined } from '@ant-design/icons';
import dayjs, { type Dayjs } from 'dayjs';
import type { ColumnsType } from 'antd/es/table';
import {
  useDailySalesReport,
  useMonthlySalesReport,
  useStockValuationReport,
  useProfitMarginReport,
  type StockValuationItem,
  type ProfitMarginItem,
} from '@/hooks/useReports';

const { Title } = Typography;

function fmt(v: number) {
  return (v ?? 0).toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
}

// ─── Daily Sales ─────────────────────────────────────────────────────────────

function DailySalesTab() {
  const [selectedDate, setSelectedDate] = useState<Dayjs>(dayjs());
  const [queriedDate, setQueriedDate] = useState<string>(dayjs().format('YYYY-MM-DD'));

  const reportQuery = useDailySalesReport(queriedDate);
  const report = reportQuery.data;

  const effectiveTax =
    report && report.subTotal > 0
      ? ((report.gstAmount / report.subTotal) * 100).toFixed(1)
      : '0.0';

  return (
    <>
      <Row gutter={8} style={{ marginBottom: 24 }}>
        <Col>
          <DatePicker
            value={selectedDate}
            onChange={(d) => d && setSelectedDate(d)}
            format="DD MMM YYYY"
            disabledDate={(d) => d.isAfter(dayjs(), 'day')}
            allowClear={false}
          />
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<SearchOutlined />}
            onClick={() => setQueriedDate(selectedDate.format('YYYY-MM-DD'))}
            loading={reportQuery.isFetching}
          >
            Load Report
          </Button>
        </Col>
      </Row>

      {reportQuery.isFetching && (
        <Spin tip="Loading report…" style={{ display: 'block', marginTop: 48 }} />
      )}

      {reportQuery.isError && !reportQuery.isFetching && (
        <Alert
          type="warning"
          message="No data available for the selected date."
          description="There may be no invoices for this date."
          showIcon
          style={{ maxWidth: 500 }}
        />
      )}

      {report && !reportQuery.isFetching && (
        <>
          <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Invoices" value={report.invoiceCount} valueStyle={{ color: '#1677ff' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Gross Sales" value={report.subTotal} precision={2} prefix="₹" valueStyle={{ color: '#3f8600' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Discount" value={report.totalDiscount} precision={2} prefix="₹" valueStyle={{ color: '#cf1322' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Net Amount" value={report.netAmount} precision={2} prefix="₹" valueStyle={{ color: '#3f8600', fontWeight: 700 }} />
              </Card>
            </Col>
          </Row>

          <Card title="GST Breakdown" size="small" style={{ maxWidth: 560 }}>
            <Descriptions column={1} size="small" bordered>
              <Descriptions.Item label="CGST"><strong>{fmt(report.totalCgst)}</strong></Descriptions.Item>
              <Descriptions.Item label="SGST"><strong>{fmt(report.totalSgst)}</strong></Descriptions.Item>
              <Descriptions.Item label="IGST"><strong>{fmt(report.totalIgst)}</strong></Descriptions.Item>
              <Descriptions.Item label="Total GST"><strong style={{ color: '#1677ff' }}>{fmt(report.gstAmount)}</strong></Descriptions.Item>
            </Descriptions>
            <Divider style={{ margin: '12px 0' }} />
            <Row justify="space-between">
              <Col style={{ color: '#888' }}>Effective Tax Rate</Col>
              <Col><strong>{effectiveTax}%</strong></Col>
            </Row>
            <Row justify="space-between" style={{ marginTop: 4 }}>
              <Col style={{ color: '#888' }}>Net Collected</Col>
              <Col><strong style={{ fontSize: 16 }}>{fmt(report.netAmount)}</strong></Col>
            </Row>
          </Card>
        </>
      )}
    </>
  );
}

// ─── Monthly Summary ──────────────────────────────────────────────────────────

function MonthlySummaryTab() {
  const [selectedMonth, setSelectedMonth] = useState<Dayjs>(dayjs());
  const [queriedYear, setQueriedYear] = useState<number>(dayjs().year());
  const [queriedMonth, setQueriedMonth] = useState<number>(dayjs().month() + 1);

  const reportQuery = useMonthlySalesReport(queriedYear, queriedMonth);
  const report = reportQuery.data;

  const effectiveTax =
    report && report.subTotal > 0
      ? ((report.gstAmount / report.subTotal) * 100).toFixed(1)
      : '0.0';

  const handleLoad = () => {
    setQueriedYear(selectedMonth.year());
    setQueriedMonth(selectedMonth.month() + 1);
  };

  return (
    <>
      <Row gutter={8} style={{ marginBottom: 24 }}>
        <Col>
          <DatePicker
            picker="month"
            value={selectedMonth}
            onChange={(d) => d && setSelectedMonth(d)}
            format="MMM YYYY"
            disabledDate={(d) => d.isAfter(dayjs(), 'month')}
            allowClear={false}
          />
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<SearchOutlined />}
            onClick={handleLoad}
            loading={reportQuery.isFetching}
          >
            Load Report
          </Button>
        </Col>
      </Row>

      {reportQuery.isFetching && (
        <Spin tip="Loading report…" style={{ display: 'block', marginTop: 48 }} />
      )}

      {reportQuery.isError && !reportQuery.isFetching && (
        <Alert
          type="warning"
          message="No data available for the selected month."
          description="There may be no invoices for this period."
          showIcon
          style={{ maxWidth: 500 }}
        />
      )}

      {report && !reportQuery.isFetching && (
        <>
          <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Invoices" value={report.invoiceCount} valueStyle={{ color: '#1677ff' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Gross Sales" value={report.subTotal} precision={2} prefix="₹" valueStyle={{ color: '#3f8600' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Discount" value={report.totalDiscount} precision={2} prefix="₹" valueStyle={{ color: '#cf1322' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Net Amount" value={report.netAmount} precision={2} prefix="₹" valueStyle={{ color: '#3f8600', fontWeight: 700 }} />
              </Card>
            </Col>
          </Row>

          <Card title="GST Breakdown" size="small" style={{ maxWidth: 560 }}>
            <Descriptions column={1} size="small" bordered>
              <Descriptions.Item label="CGST"><strong>{fmt(report.totalCgst)}</strong></Descriptions.Item>
              <Descriptions.Item label="SGST"><strong>{fmt(report.totalSgst)}</strong></Descriptions.Item>
              <Descriptions.Item label="IGST"><strong>{fmt(report.totalIgst)}</strong></Descriptions.Item>
              <Descriptions.Item label="Total GST"><strong style={{ color: '#1677ff' }}>{fmt(report.gstAmount)}</strong></Descriptions.Item>
            </Descriptions>
            <Divider style={{ margin: '12px 0' }} />
            <Row justify="space-between">
              <Col style={{ color: '#888' }}>Effective Tax Rate</Col>
              <Col><strong>{effectiveTax}%</strong></Col>
            </Row>
            <Row justify="space-between" style={{ marginTop: 4 }}>
              <Col style={{ color: '#888' }}>Net Collected</Col>
              <Col><strong style={{ fontSize: 16 }}>{fmt(report.netAmount)}</strong></Col>
            </Row>
          </Card>
        </>
      )}
    </>
  );
}

// ─── Stock Valuation ──────────────────────────────────────────────────────────

const stockColumns: ColumnsType<StockValuationItem> = [
  {
    title: 'Drug',
    dataIndex: 'drugName',
    key: 'drugName',
    render: (v: string | null) => v ?? '—',
    sorter: (a, b) => (a.drugName ?? '').localeCompare(b.drugName ?? ''),
  },
  {
    title: 'HSN Code',
    dataIndex: 'hsnCode',
    key: 'hsnCode',
    render: (v: string | null) =>
      v ? <Tag style={{ fontFamily: 'monospace' }}>{v}</Tag> : <span style={{ color: '#bbb' }}>—</span>,
  },
  {
    title: 'Qty in Stock',
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    align: 'right',
    sorter: (a, b) => a.totalQuantity - b.totalQuantity,
  },
  {
    title: 'MRP',
    dataIndex: 'mrp',
    key: 'mrp',
    align: 'right',
    render: (v: number) => fmt(v),
    sorter: (a, b) => a.mrp - b.mrp,
  },
  {
    title: 'MRP Value',
    dataIndex: 'totalMrpValue',
    key: 'totalMrpValue',
    align: 'right',
    render: (v: number) => <strong>{fmt(v)}</strong>,
    sorter: (a, b) => a.totalMrpValue - b.totalMrpValue,
    defaultSortOrder: 'descend',
  },
  {
    title: 'Cost Value',
    dataIndex: 'totalCostValue',
    key: 'totalCostValue',
    align: 'right',
    render: (v: number | null) =>
      v != null ? fmt(v) : <span style={{ color: '#bbb' }}>N/A</span>,
    sorter: (a, b) => (a.totalCostValue ?? 0) - (b.totalCostValue ?? 0),
  },
];

function StockValuationTab() {
  const reportQuery = useStockValuationReport();
  const items = reportQuery.data ?? [];

  const totalMrpValue = items.reduce((s, i) => s + i.totalMrpValue, 0);
  const totalCostValue = items.reduce((s, i) => s + (i.totalCostValue ?? 0), 0);
  const totalQty = items.reduce((s, i) => s + i.totalQuantity, 0);

  return (
    <>
      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col>
          <Button
            icon={<ReloadOutlined />}
            onClick={() => reportQuery.refetch()}
            loading={reportQuery.isFetching}
          >
            Refresh
          </Button>
        </Col>
      </Row>

      {reportQuery.isError && !reportQuery.isFetching && (
        <Alert
          type="warning"
          message="Could not load stock valuation."
          showIcon
          style={{ maxWidth: 500, marginBottom: 16 }}
        />
      )}

      {!reportQuery.isError && (
        <>
          <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="SKUs" value={items.length} valueStyle={{ color: '#1677ff' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Total Units" value={totalQty} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="MRP Value" value={totalMrpValue} precision={2} prefix="₹" valueStyle={{ color: '#3f8600' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Cost Value" value={totalCostValue} precision={2} prefix="₹" valueStyle={{ color: '#1677ff' }} />
              </Card>
            </Col>
          </Row>

          <Table<StockValuationItem>
            rowKey="drugId"
            columns={stockColumns}
            dataSource={items}
            loading={reportQuery.isFetching}
            size="small"
            pagination={{ pageSize: 30, showSizeChanger: true, showTotal: (t) => `${t} items` }}
            scroll={{ x: 700 }}
          />
        </>
      )}
    </>
  );
}

// ─── Profit Margin ────────────────────────────────────────────────────────────

const marginColumns: ColumnsType<ProfitMarginItem> = [
  {
    title: 'Drug',
    dataIndex: 'drugName',
    render: (v: string | null) => v ?? '—',
    sorter: (a, b) => (a.drugName ?? '').localeCompare(b.drugName ?? ''),
  },
  {
    title: 'HSN',
    dataIndex: 'hsnCode',
    width: 90,
    render: (v: string | null) => v ? <Tag style={{ fontFamily: 'monospace' }}>{v}</Tag> : <span style={{ color: '#bbb' }}>—</span>,
  },
  {
    title: 'Qty Sold',
    dataIndex: 'totalQtySold',
    align: 'right',
    width: 90,
    sorter: (a, b) => a.totalQtySold - b.totalQtySold,
  },
  {
    title: 'MRP',
    dataIndex: 'mrp',
    align: 'right',
    width: 100,
    render: (v: number) => fmt(v),
    sorter: (a, b) => a.mrp - b.mrp,
  },
  {
    title: 'Avg Cost',
    dataIndex: 'averageCostPrice',
    align: 'right',
    width: 100,
    render: (v: number | null) => v != null ? fmt(v) : <span style={{ color: '#bbb' }}>N/A</span>,
    sorter: (a, b) => (a.averageCostPrice ?? 0) - (b.averageCostPrice ?? 0),
  },
  {
    title: 'Revenue',
    dataIndex: 'totalRevenue',
    align: 'right',
    width: 110,
    render: (v: number) => fmt(v),
    sorter: (a, b) => a.totalRevenue - b.totalRevenue,
  },
  {
    title: 'Cost',
    dataIndex: 'totalCost',
    align: 'right',
    width: 110,
    render: (v: number) => fmt(v),
    sorter: (a, b) => a.totalCost - b.totalCost,
  },
  {
    title: 'Gross Profit',
    dataIndex: 'grossProfit',
    align: 'right',
    width: 120,
    defaultSortOrder: 'descend',
    render: (v: number) => (
      <strong style={{ color: v >= 0 ? '#3f8600' : '#cf1322' }}>{fmt(v)}</strong>
    ),
    sorter: (a, b) => a.grossProfit - b.grossProfit,
  },
  {
    title: 'Margin %',
    dataIndex: 'marginPct',
    align: 'right',
    width: 100,
    render: (v: number) => {
      const color = v >= 20 ? '#3f8600' : v >= 10 ? '#d46b08' : '#cf1322';
      return <Tag color={color}>{v.toFixed(1)}%</Tag>;
    },
    sorter: (a, b) => a.marginPct - b.marginPct,
  },
  {
    title: 'MRP Margin %',
    dataIndex: 'mrpMarginPct',
    align: 'right',
    width: 115,
    render: (v: number | null) => {
      if (v == null) return <span style={{ color: '#bbb' }}>N/A</span>;
      const color = v >= 20 ? '#3f8600' : v >= 10 ? '#d46b08' : '#cf1322';
      return <Tag color={color}>{v.toFixed(1)}%</Tag>;
    },
    sorter: (a, b) => (a.mrpMarginPct ?? 0) - (b.mrpMarginPct ?? 0),
  },
];

function ProfitMarginTab() {
  const [range, setRange] = useState<[Dayjs, Dayjs]>([dayjs().startOf('month'), dayjs()]);
  const [queriedRange, setQueriedRange] = useState<[string, string]>([
    dayjs().startOf('month').format('YYYY-MM-DD'),
    dayjs().format('YYYY-MM-DD'),
  ]);

  const reportQuery = useProfitMarginReport(queriedRange[0], queriedRange[1]);
  const items = reportQuery.data ?? [];

  const totalRevenue = items.reduce((s, i) => s + i.totalRevenue, 0);
  const totalProfit = items.reduce((s, i) => s + i.grossProfit, 0);
  const overallMargin = totalRevenue > 0 ? (totalProfit / totalRevenue) * 100 : 0;

  return (
    <>
      <Row gutter={8} style={{ marginBottom: 24 }}>
        <Col>
          <DatePicker.RangePicker
            value={range}
            onChange={(v) => v && setRange(v as [Dayjs, Dayjs])}
            format="DD MMM YYYY"
            disabledDate={(d) => d.isAfter(dayjs(), 'day')}
            allowClear={false}
          />
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<SearchOutlined />}
            onClick={() => setQueriedRange([range[0].format('YYYY-MM-DD'), range[1].format('YYYY-MM-DD')])}
            loading={reportQuery.isFetching}
          >
            Load Report
          </Button>
        </Col>
      </Row>

      {reportQuery.isError && !reportQuery.isFetching && (
        <Alert type="warning" message="No sales data found for this period." showIcon style={{ maxWidth: 500, marginBottom: 16 }} />
      )}

      {!reportQuery.isError && (
        <>
          <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="SKUs Sold" value={items.length} valueStyle={{ color: '#1677ff' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Total Revenue" value={totalRevenue} precision={2} prefix="₹" valueStyle={{ color: '#3f8600' }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Gross Profit" value={totalProfit} precision={2} prefix="₹"
                  valueStyle={{ color: totalProfit >= 0 ? '#3f8600' : '#cf1322', fontWeight: 700 }} />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic title="Overall Margin" value={overallMargin} precision={1} suffix="%"
                  valueStyle={{ color: overallMargin >= 15 ? '#3f8600' : overallMargin >= 8 ? '#d46b08' : '#cf1322', fontWeight: 700 }} />
              </Card>
            </Col>
          </Row>

          <Table<ProfitMarginItem>
            rowKey="drugId"
            columns={marginColumns}
            dataSource={items}
            loading={reportQuery.isFetching}
            size="small"
            pagination={{ pageSize: 30, showSizeChanger: true, showTotal: (t) => `${t} drugs` }}
            scroll={{ x: 1000 }}
          />
        </>
      )}
    </>
  );
}

// ─── Page ─────────────────────────────────────────────────────────────────────

export default function ReportsPage() {
  return (
    <div style={{ padding: 24 }}>
      <Row align="middle" style={{ marginBottom: 24 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            <BarChartOutlined style={{ marginRight: 8 }} />
            Reports
          </Title>
        </Col>
      </Row>

      <Tabs
        defaultActiveKey="daily"
        items={[
          { key: 'daily', label: 'Daily Sales', children: <DailySalesTab /> },
          { key: 'monthly', label: 'Monthly Summary', children: <MonthlySummaryTab /> },
          { key: 'stock', label: 'Stock Valuation', children: <StockValuationTab /> },
          { key: 'margin', label: 'Profit Margin', children: <ProfitMarginTab /> },
        ]}
      />
    </div>
  );
}
