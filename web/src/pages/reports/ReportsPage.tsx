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
} from 'antd';
import { SearchOutlined, BarChartOutlined } from '@ant-design/icons';
import dayjs, { type Dayjs } from 'dayjs';
import { useDailySalesReport } from '@/hooks/useReports';

const { Title } = Typography;

function fmt(v: number) {
  return (v ?? 0).toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
}

export default function ReportsPage() {
  const [selectedDate, setSelectedDate] = useState<Dayjs>(dayjs());
  const [queriedDate, setQueriedDate] = useState<string>(dayjs().format('YYYY-MM-DD'));

  const reportQuery = useDailySalesReport(queriedDate);
  const report = reportQuery.data;

  const effectiveTax = report && report.subTotal > 0
    ? ((report.gstAmount / report.subTotal) * 100).toFixed(1)
    : '0.0';

  return (
    <div style={{ padding: 24 }}>
      <Row align="middle" style={{ marginBottom: 24 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            <BarChartOutlined style={{ marginRight: 8 }} />
            Reports — Daily Sales
          </Title>
        </Col>
      </Row>

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
          description="The report could not be loaded. There may be no invoices for this date."
          showIcon
          style={{ maxWidth: 500 }}
        />
      )}

      {report && !reportQuery.isFetching && (
        <>
          <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic
                  title="Invoices"
                  value={report.invoiceCount}
                  valueStyle={{ color: '#1677ff' }}
                />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic
                  title="Gross Sales"
                  value={report.subTotal}
                  precision={2}
                  prefix="₹"
                  valueStyle={{ color: '#3f8600' }}
                />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic
                  title="Discount"
                  value={report.totalDiscount}
                  precision={2}
                  prefix="₹"
                  valueStyle={{ color: '#cf1322' }}
                />
              </Card>
            </Col>
            <Col xs={12} sm={6}>
              <Card size="small">
                <Statistic
                  title="Net Amount"
                  value={report.netAmount}
                  precision={2}
                  prefix="₹"
                  valueStyle={{ color: '#3f8600', fontWeight: 700 }}
                />
              </Card>
            </Col>
          </Row>

          <Card
            title="GST Breakdown"
            size="small"
            style={{ maxWidth: 560 }}
          >
            <Descriptions column={1} size="small" bordered>
              <Descriptions.Item label="CGST">
                <strong>{fmt(report.totalCgst)}</strong>
              </Descriptions.Item>
              <Descriptions.Item label="SGST">
                <strong>{fmt(report.totalSgst)}</strong>
              </Descriptions.Item>
              <Descriptions.Item label="IGST">
                <strong>{fmt(report.totalIgst)}</strong>
              </Descriptions.Item>
              <Descriptions.Item label="Total GST">
                <strong style={{ color: '#1677ff' }}>{fmt(report.gstAmount)}</strong>
              </Descriptions.Item>
            </Descriptions>
            <Divider style={{ margin: '12px 0' }} />
            <Row justify="space-between">
              <Col style={{ color: '#888' }}>Effective Tax Rate</Col>
              <Col>
                <strong>{effectiveTax}%</strong>
              </Col>
            </Row>
            <Row justify="space-between" style={{ marginTop: 4 }}>
              <Col style={{ color: '#888' }}>Net Collected</Col>
              <Col>
                <strong style={{ fontSize: 16 }}>{fmt(report.netAmount)}</strong>
              </Col>
            </Row>
          </Card>
        </>
      )}
    </div>
  );
}
