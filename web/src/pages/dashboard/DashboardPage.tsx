import { useMemo } from 'react';
import { Row, Col, Card, Statistic, Table, Tag, Typography, Alert, Spin } from 'antd';
import {
  MedicineBoxOutlined,
  ShoppingCartOutlined,
  TeamOutlined,
  WarningOutlined,
  BarChartOutlined,
  ContactsOutlined,
  CreditCardOutlined,
  RiseOutlined,
} from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import type { ColumnsType } from 'antd/es/table';
import { useDrugInventory } from '@/hooks/useDrugInventory';
import { useDrugs } from '@/hooks/useDrugs';
import { useVendors } from '@/hooks/useVendors';
import { usePurchaseOrders } from '@/hooks/usePurchaseOrders';
import { useExpiryRecords } from '@/hooks/useExpiryRecords';
import { useDailySalesReport, useMonthlySalesReport, useStockValuationReport } from '@/hooks/useReports';
import { usePatients } from '@/hooks/usePatients';
import { useDebtRecords } from '@/hooks/useDebtRecords';
import type { DrugInventory } from '@/api/localTypes';

const { Title } = Typography;

const WARN_DAYS = 90;

function StatCard({
  title,
  value,
  icon,
  color,
  onClick,
  prefix,
}: {
  title: string;
  value: number | string;
  icon: React.ReactNode;
  color?: string;
  onClick?: () => void;
  prefix?: string;
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
        prefix={
          <>
            <span style={{ color: color ?? '#1677ff', marginRight: 4 }}>{icon}</span>
            {prefix}
          </>
        }
        valueStyle={{ color: color ?? '#1677ff' }}
      />
    </Card>
  );
}

export default function DashboardPage() {
  const navigate = useNavigate();
  const today = dayjs().format('YYYY-MM-DD');
  const currentYear = dayjs().year();
  const currentMonth = dayjs().month() + 1;

  const inventoryQuery = useDrugInventory();
  const drugsQuery = useDrugs();
  const vendorsQuery = useVendors();
  const ordersQuery = usePurchaseOrders();
  const expiryQuery = useExpiryRecords();
  const salesQuery = useDailySalesReport(today);
  const monthlyQuery = useMonthlySalesReport(currentYear, currentMonth);
  const stockValuationQuery = useStockValuationReport();
  const patientsQuery = usePatients();
  const debtQuery = useDebtRecords();

  const drugMap = useMemo(() => {
    const map = new Map<string, { name: string; reorderLevel: number }>();
    for (const d of drugsQuery.data ?? []) {
      if (d.id) map.set(d.id, { name: d.name ?? d.id, reorderLevel: d.reorderLevel ?? 10 });
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
      .filter((inv) => {
        const drug = drugMap.get(inv.drugId ?? '');
        const threshold = drug?.reorderLevel ?? 10;
        return inv.quantityInStock <= threshold && inv.quantityInStock > 0;
      })
      .sort((a, b) => a.quantityInStock - b.quantityInStock)
      .slice(0, 10);
  }, [inventoryQuery.data, drugMap]);

  const pendingOrders = (ordersQuery.data ?? []).filter(
    (o) => o.status === 'Pending' || o.status === 'Draft',
  ).length;

  const openExpiryAlerts = (expiryQuery.data ?? []).filter(
    (r) => r.status === 'Detected' || r.status === 'Pending',
  ).length;

  const totalOutstandingDebt = (debtQuery.data ?? [])
    .filter((d) => d.remainingAmount > 0)
    .reduce((s, d) => s + d.remainingAmount, 0);

  const overdueDebts = (debtQuery.data ?? []).filter(
    (d) => d.dueDate && dayjs(d.dueDate).isBefore(dayjs(), 'day') && d.remainingAmount > 0,
  ).length;

  const totalMrpValue = (stockValuationQuery.data ?? []).reduce(
    (s, i) => s + i.totalMrpValue, 0,
  );

  const expiryColumns: ColumnsType<DrugInventory> = [
    {
      title: 'Drug',
      dataIndex: 'drugId',
      key: 'drug',
      render: (id: string) => drugMap.get(id)?.name ?? id,
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
      render: (id: string) => drugMap.get(id)?.name ?? id,
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
      render: (v: number) => <Tag color={v === 0 ? 'red' : 'orange'}>{v}</Tag>,
    },
    {
      title: 'Reorder At',
      dataIndex: 'drugId',
      key: 'reorder',
      align: 'right',
      render: (id: string) => drugMap.get(id)?.reorderLevel ?? 10,
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
          {/* Today's sales + month revenue */}
          <Row gutter={16} style={{ marginBottom: 16 }}>
            {salesQuery.data && (
              <Col xs={24} lg={12}>
                <Card
                  size="small"
                  title={
                    <span>
                      <BarChartOutlined style={{ marginRight: 6, color: '#1677ff' }} />
                      Today's Sales — {dayjs().format('DD MMM YYYY')}
                    </span>
                  }
                >
                  <Row gutter={16}>
                    <Col span={8}>
                      <Statistic title="Invoices" value={salesQuery.data.invoiceCount} />
                    </Col>
                    <Col span={8}>
                      <Statistic title="Gross" value={salesQuery.data.subTotal} precision={2} prefix="₹" />
                    </Col>
                    <Col span={8}>
                      <Statistic
                        title="Net"
                        value={salesQuery.data.netAmount}
                        precision={2}
                        prefix="₹"
                        valueStyle={{ color: '#3f8600', fontWeight: 700 }}
                      />
                    </Col>
                  </Row>
                </Card>
              </Col>
            )}
            {monthlyQuery.data && (
              <Col xs={24} lg={12}>
                <Card
                  size="small"
                  title={
                    <span>
                      <RiseOutlined style={{ marginRight: 6, color: '#722ed1' }} />
                      {dayjs().format('MMMM YYYY')} Revenue
                    </span>
                  }
                >
                  <Row gutter={16}>
                    <Col span={8}>
                      <Statistic title="Invoices" value={monthlyQuery.data.invoiceCount} />
                    </Col>
                    <Col span={8}>
                      <Statistic title="Gross" value={monthlyQuery.data.subTotal} precision={2} prefix="₹" />
                    </Col>
                    <Col span={8}>
                      <Statistic
                        title="Net"
                        value={monthlyQuery.data.netAmount}
                        precision={2}
                        prefix="₹"
                        valueStyle={{ color: '#722ed1', fontWeight: 700 }}
                      />
                    </Col>
                  </Row>
                </Card>
              </Col>
            )}
          </Row>

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
            <Col xs={12} sm={8} md={6}>
              <StatCard
                title="Patients"
                value={(patientsQuery.data ?? []).length}
                icon={<ContactsOutlined />}
                onClick={() => navigate('/patients')}
              />
            </Col>
            <Col xs={12} sm={8} md={6}>
              <StatCard
                title="Outstanding Debt"
                value={totalOutstandingDebt.toLocaleString('en-IN', { minimumFractionDigits: 2 })}
                icon={<CreditCardOutlined />}
                color={totalOutstandingDebt > 0 ? '#cf1322' : '#3f8600'}
                prefix="₹"
                onClick={() => navigate('/debt')}
              />
            </Col>
            {overdueDebts > 0 && (
              <Col xs={12} sm={8} md={6}>
                <StatCard
                  title="Overdue Debts"
                  value={overdueDebts}
                  icon={<CreditCardOutlined />}
                  color="#cf1322"
                  onClick={() => navigate('/debt')}
                />
              </Col>
            )}
            {stockValuationQuery.data && (
              <Col xs={12} sm={8} md={6}>
                <StatCard
                  title="Stock Value (MRP)"
                  value={totalMrpValue.toLocaleString('en-IN', { minimumFractionDigits: 2 })}
                  icon={<BarChartOutlined />}
                  color="#1677ff"
                  prefix="₹"
                  onClick={() => navigate('/reports')}
                />
              </Col>
            )}
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

          {overdueDebts > 0 && (
            <Alert
              type="warning"
              showIcon
              icon={<CreditCardOutlined />}
              message={`${overdueDebts} debt record${overdueDebts > 1 ? 's are' : ' is'} overdue`}
              action={
                <a onClick={() => navigate('/debt')} style={{ whiteSpace: 'nowrap' }}>
                  View →
                </a>
              }
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
                extra={<a onClick={() => navigate('/inventory/drugs')}>See all</a>}
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
            <Col xs={24} lg={12}>
              <Card
                size="small"
                title={
                  <span>
                    <WarningOutlined style={{ color: '#ff4d4f', marginRight: 6 }} />
                    Low Stock (at or below reorder level)
                  </span>
                }
                extra={<a onClick={() => navigate('/inventory/drugs')}>See all</a>}
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
