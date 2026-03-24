import { useMemo } from 'react';
import { Row, Col, Card, Statistic, Table, Tag, Typography, Alert, Spin } from 'antd';
import {
  MedicineBoxOutlined,
  ShoppingCartOutlined,
  TeamOutlined,
  WarningOutlined,
  BarChartOutlined,
} from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import type { ColumnsType } from 'antd/es/table';
import { useDrugInventory } from '@/hooks/useDrugInventory';
import { useDrugs } from '@/hooks/useDrugs';
import { useVendors } from '@/hooks/useVendors';
import { usePurchaseOrders } from '@/hooks/usePurchaseOrders';
import { useExpiryRecords } from '@/hooks/useExpiryRecords';
import { useDailySalesReport } from '@/hooks/useReports';
import type { DrugInventory } from '@/api/localTypes';

const { Title } = Typography;

const WARN_DAYS = 90;

function StatCard({
  title,
  value,
  icon,
  color,
  onClick,
}: {
  title: string;
  value: number | string;
  icon: React.ReactNode;
  color?: string;
  onClick?: () => void;
}) {
  return (
    <Card
      size="small"
      hoverable={!!onClick}
      onClick={onClick}
      style={{ cursor: onClick ? 'pointer' : 'default' }}
    >
      <Statistic
        title={title}
        value={value}
        prefix={<span style={{ color: color ?? '#1677ff', marginRight: 4 }}>{icon}</span>}
        valueStyle={{ color: color ?? '#1677ff' }}
      />
    </Card>
  );
}

export default function DashboardPage() {
  const navigate = useNavigate();
  const today = dayjs().format('YYYY-MM-DD');

  const inventoryQuery = useDrugInventory();
  const drugsQuery = useDrugs();
  const vendorsQuery = useVendors();
  const ordersQuery = usePurchaseOrders();
  const expiryQuery = useExpiryRecords();
  const salesQuery = useDailySalesReport(today);

  const drugMap = useMemo(() => {
    const map = new Map<string, string>();
    for (const d of drugsQuery.data ?? []) {
      if (d.id) map.set(d.id, d.name ?? d.id);
    }
    return map;
  }, [drugsQuery.data]);

  const expiringSoon = useMemo(() => {
    const threshold = dayjs().add(WARN_DAYS, 'day');
    return (inventoryQuery.data ?? [])
      .filter((inv) => {
        const exp = dayjs(inv.expirationDate);
        return exp.isBefore(threshold) && inv.quantityInStock > 0;
      })
      .sort((a, b) => dayjs(a.expirationDate).unix() - dayjs(b.expirationDate).unix())
      .slice(0, 10);
  }, [inventoryQuery.data]);

  const lowStock = useMemo(() => {
    return (inventoryQuery.data ?? [])
      .filter((inv) => inv.quantityInStock <= 10 && inv.quantityInStock > 0)
      .sort((a, b) => a.quantityInStock - b.quantityInStock)
      .slice(0, 10);
  }, [inventoryQuery.data]);

  const pendingOrders = (ordersQuery.data ?? []).filter(
    (o) => o.status === 'Pending' || o.status === 'Draft',
  ).length;

  const openExpiryAlerts = (expiryQuery.data ?? []).filter(
    (r) => r.status === 'Detected' || r.status === 'Pending',
  ).length;

  const expiryColumns: ColumnsType<DrugInventory> = [
    {
      title: 'Drug',
      dataIndex: 'drugId',
      key: 'drug',
      render: (id: string) => drugMap.get(id) ?? id,
    },
    {
      title: 'Batch',
      dataIndex: 'batchNumber',
      key: 'batchNumber',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Qty',
      dataIndex: 'quantityInStock',
      key: 'qty',
      align: 'right',
    },
    {
      title: 'Expires',
      dataIndex: 'expirationDate',
      key: 'expirationDate',
      render: (v: string) => {
        const d = dayjs(v);
        const isExpired = d.isBefore(dayjs());
        const daysLeft = d.diff(dayjs(), 'day');
        return (
          <Tag color={isExpired ? 'red' : daysLeft <= 30 ? 'orange' : 'gold'}>
            {isExpired ? `Expired ${d.format('DD MMM')}` : `${daysLeft}d — ${d.format('DD MMM YYYY')}`}
          </Tag>
        );
      },
    },
  ];

  const lowStockColumns: ColumnsType<DrugInventory> = [
    {
      title: 'Drug',
      dataIndex: 'drugId',
      key: 'drug',
      render: (id: string) => drugMap.get(id) ?? id,
    },
    {
      title: 'Batch',
      dataIndex: 'batchNumber',
      key: 'batchNumber',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Qty in Stock',
      dataIndex: 'quantityInStock',
      key: 'qty',
      align: 'right',
      render: (v: number) => <Tag color={v === 0 ? 'red' : 'orange'}>{v}</Tag>,
    },
  ];

  const isLoading =
    inventoryQuery.isLoading || drugsQuery.isLoading || vendorsQuery.isLoading || ordersQuery.isLoading;

  return (
    <div style={{ padding: 24 }}>
      <Title level={4} style={{ marginBottom: 20 }}>
        Dashboard
      </Title>

      {isLoading ? (
        <Spin tip="Loading overview…" style={{ display: 'block', marginTop: 48 }} />
      ) : (
        <>
          {/* Today's sales summary */}
          {salesQuery.data && (
            <Card
              size="small"
              title={
                <span>
                  <BarChartOutlined style={{ marginRight: 6, color: '#1677ff' }} />
                  Today's Sales — {dayjs().format('DD MMM YYYY')}
                </span>
              }
              style={{ marginBottom: 16 }}
            >
              <Row gutter={16}>
                <Col xs={12} sm={6}>
                  <Statistic title="Invoices" value={salesQuery.data.invoiceCount} />
                </Col>
                <Col xs={12} sm={6}>
                  <Statistic
                    title="Gross Sales"
                    value={salesQuery.data.subTotal}
                    precision={2}
                    prefix="₹"
                  />
                </Col>
                <Col xs={12} sm={6}>
                  <Statistic
                    title="Total GST"
                    value={salesQuery.data.gstAmount}
                    precision={2}
                    prefix="₹"
                  />
                </Col>
                <Col xs={12} sm={6}>
                  <Statistic
                    title="Net Amount"
                    value={salesQuery.data.netAmount}
                    precision={2}
                    prefix="₹"
                    valueStyle={{ color: '#3f8600', fontWeight: 700 }}
                  />
                </Col>
              </Row>
            </Card>
          )}

          {/* KPI cards */}
          <Row gutter={[16, 16]} style={{ marginBottom: 20 }}>
            <Col xs={12} sm={8} md={6}>
              <StatCard
                title="Total Drugs"
                value={(drugsQuery.data ?? []).length}
                icon={<MedicineBoxOutlined />}
                onClick={() => navigate('/drugs')}
              />
            </Col>
            <Col xs={12} sm={8} md={6}>
              <StatCard
                title="Inventory Lines"
                value={(inventoryQuery.data ?? []).length}
                icon={<MedicineBoxOutlined />}
                onClick={() => navigate('/inventory/drugs')}
              />
            </Col>
            <Col xs={12} sm={8} md={6}>
              <StatCard
                title="Vendors"
                value={(vendorsQuery.data ?? []).length}
                icon={<TeamOutlined />}
                onClick={() => navigate('/vendors')}
              />
            </Col>
            <Col xs={12} sm={8} md={6}>
              <StatCard
                title="Pending Orders"
                value={pendingOrders}
                icon={<ShoppingCartOutlined />}
                color={pendingOrders > 0 ? '#d46b08' : '#3f8600'}
                onClick={() => navigate('/purchase-orders')}
              />
            </Col>
          </Row>

          {/* Alert banners */}
          {openExpiryAlerts > 0 && (
            <Alert
              type="error"
              showIcon
              icon={<WarningOutlined />}
              message={`${openExpiryAlerts} open expiry alert${openExpiryAlerts > 1 ? 's' : ''} require attention`}
              action={
                <a onClick={() => navigate('/expired-drugs')} style={{ whiteSpace: 'nowrap' }}>
                  View →
                </a>
              }
              style={{ marginBottom: 16 }}
            />
          )}

          {expiringSoon.length > 0 && (
            <Alert
              type="warning"
              showIcon
              icon={<WarningOutlined />}
              message={`${expiringSoon.length} inventory item${expiringSoon.length > 1 ? 's' : ''} expiring within ${WARN_DAYS} days`}
              style={{ marginBottom: 16 }}
            />
          )}

          {/* Detail tables */}
          <Row gutter={16}>
            <Col xs={24} lg={12}>
              <Card
                size="small"
                title={
                  <span>
                    <WarningOutlined style={{ color: '#faad14', marginRight: 6 }} />
                    Expiring Soon (next {WARN_DAYS} days)
                  </span>
                }
                extra={
                  <a onClick={() => navigate('/inventory/drugs')}>See all</a>
                }
              >
                {expiringSoon.length === 0 ? (
                  <div style={{ color: '#3f8600', padding: '8px 0' }}>No items expiring soon</div>
                ) : (
                  <Table<DrugInventory>
                    rowKey="id"
                    columns={expiryColumns}
                    dataSource={expiringSoon}
                    size="small"
                    pagination={false}
                    scroll={{ x: 400 }}
                  />
                )}
              </Card>
            </Col>
            <Col xs={24} lg={12} style={{ marginTop: 0 }}>
              <Card
                size="small"
                title={
                  <span>
                    <WarningOutlined style={{ color: '#ff4d4f', marginRight: 6 }} />
                    Low Stock (≤ 10 units)
                  </span>
                }
                extra={
                  <a onClick={() => navigate('/inventory/drugs')}>See all</a>
                }
              >
                {lowStock.length === 0 ? (
                  <div style={{ color: '#3f8600', padding: '8px 0' }}>No low stock items</div>
                ) : (
                  <Table<DrugInventory>
                    rowKey="id"
                    columns={lowStockColumns}
                    dataSource={lowStock}
                    size="small"
                    pagination={false}
                    scroll={{ x: 400 }}
                  />
                )}
              </Card>
            </Col>
          </Row>
        </>
      )}
    </div>
  );
}
