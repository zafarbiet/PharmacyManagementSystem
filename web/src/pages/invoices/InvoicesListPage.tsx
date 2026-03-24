import { useState, useMemo } from 'react';
import { Table, Input, Select, Button, Tag, Space, Typography, Row, Col, Card, Tooltip, Popconfirm, message } from 'antd';
import { SearchOutlined, PlusOutlined, ReloadOutlined, InfoCircleOutlined, DeleteOutlined, EyeOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { CustomerInvoice } from '@/api/localTypes';
import { useCustomerInvoices } from '@/hooks/useCustomerInvoices';
import { useDeleteCustomerInvoice } from '@/hooks/useCustomerInvoiceMutations';
import InvoiceFormModal from '@/components/InvoiceFormModal';
import InvoiceDetailDrawer from '@/components/InvoiceDetailDrawer';

const { Title } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Draft: 'default',
  Pending: 'blue',
  Paid: 'green',
  PartiallyPaid: 'orange',
  Cancelled: 'red',
  Refunded: 'purple',
};

const STATUS_OPTIONS = Object.keys(STATUS_COLORS).map((s) => ({ value: s, label: s }));

const PAYMENT_COLORS: Record<string, string> = {
  Cash: 'green',
  UPI: 'blue',
  Card: 'geekblue',
  Credit: 'orange',
  NEFT: 'cyan',
};

function fmt(v: number | undefined) {
  return (v ?? 0).toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
}

export default function InvoicesListPage() {
  const [search, setSearch] = useState('');
  const [appliedSearch, setAppliedSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState<string | undefined>();
  const [modalOpen, setModalOpen] = useState(false);
  const [viewInvoice, setViewInvoice] = useState<CustomerInvoice | null>(null);

  const invoicesQuery = useCustomerInvoices();
  const deleteInvoice = useDeleteCustomerInvoice();
  const invoices = (invoicesQuery.data ?? []) as unknown as CustomerInvoice[];

  const filtered = useMemo(() => {
    let rows = invoices;
    if (appliedSearch) {
      const lower = appliedSearch.toLowerCase();
      rows = rows.filter(
        (inv) =>
          (inv.invoiceNumber ?? '').toLowerCase().includes(lower) ||
          (inv.billedBy ?? '').toLowerCase().includes(lower) ||
          (inv.paymentMethod ?? '').toLowerCase().includes(lower),
      );
    }
    if (statusFilter) {
      rows = rows.filter((inv) => inv.status === statusFilter);
    }
    return rows;
  }, [invoices, appliedSearch, statusFilter]);

  const totalNet = filtered.reduce((s, inv) => s + (inv.netAmount ?? 0), 0);
  const totalGst = filtered.reduce((s, inv) => s + (inv.totalCgst ?? 0) + (inv.totalSgst ?? 0) + (inv.totalIgst ?? 0), 0);
  const paidCount = filtered.filter((inv) => inv.status === 'Paid').length;

  const handleDelete = async (id: string) => {
    try {
      await deleteInvoice.mutateAsync(id);
      message.success('Invoice deleted.');
    } catch {
      message.error('Failed to delete invoice.');
    }
  };

  const columns: ColumnsType<CustomerInvoice> = [
    {
      title: 'Invoice #',
      dataIndex: 'invoiceNumber',
      key: 'invoiceNumber',
      render: (v: string | null) => <strong>{v ?? '—'}</strong>,
      sorter: (a, b) => (a.invoiceNumber ?? '').localeCompare(b.invoiceNumber ?? ''),
    },
    {
      title: 'Date',
      dataIndex: 'invoiceDate',
      key: 'invoiceDate',
      render: (v: string) => (v ? dayjs(v).format('DD MMM YYYY') : '—'),
      sorter: (a, b) => dayjs(a.invoiceDate).unix() - dayjs(b.invoiceDate).unix(),
      defaultSortOrder: 'descend',
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: (v: string | null) => (
        <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>
      ),
    },
    {
      title: 'Payment',
      dataIndex: 'paymentMethod',
      key: 'paymentMethod',
      render: (v: string | null) =>
        v ? <Tag color={PAYMENT_COLORS[v] ?? 'default'}>{v}</Tag> : <span style={{ color: '#bbb' }}>—</span>,
    },
    {
      title: 'Sub Total',
      dataIndex: 'subTotal',
      key: 'subTotal',
      align: 'right',
      render: (v: number) => fmt(v),
      sorter: (a, b) => (a.subTotal ?? 0) - (b.subTotal ?? 0),
    },
    {
      title: 'Discount',
      dataIndex: 'discountAmount',
      key: 'discountAmount',
      align: 'right',
      render: (v: number) => (v ? <span style={{ color: '#cf1322' }}>{fmt(v)}</span> : '—'),
    },
    {
      title: () => (
        <Tooltip title="CGST + SGST + IGST">
          <span>
            GST <InfoCircleOutlined style={{ fontSize: 11, color: '#888' }} />
          </span>
        </Tooltip>
      ),
      key: 'gst',
      align: 'right',
      render: (_: unknown, inv: CustomerInvoice) => {
        const total = (inv.totalCgst ?? 0) + (inv.totalSgst ?? 0) + (inv.totalIgst ?? 0);
        return (
          <Tooltip
            title={
              <div>
                <div>CGST: {fmt(inv.totalCgst)}</div>
                <div>SGST: {fmt(inv.totalSgst)}</div>
                <div>IGST: {fmt(inv.totalIgst)}</div>
              </div>
            }
          >
            <span style={{ color: '#1677ff', cursor: 'default' }}>{fmt(total)}</span>
          </Tooltip>
        );
      },
      sorter: (a, b) => {
        const gstA = (a.totalCgst ?? 0) + (a.totalSgst ?? 0) + (a.totalIgst ?? 0);
        const gstB = (b.totalCgst ?? 0) + (b.totalSgst ?? 0) + (b.totalIgst ?? 0);
        return gstA - gstB;
      },
    },
    {
      title: 'Net Amount',
      dataIndex: 'netAmount',
      key: 'netAmount',
      align: 'right',
      render: (v: number) => <strong style={{ color: '#3f8600' }}>{fmt(v)}</strong>,
      sorter: (a, b) => (a.netAmount ?? 0) - (b.netAmount ?? 0),
    },
    {
      title: 'Billed By',
      dataIndex: 'billedBy',
      key: 'billedBy',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Branch GST',
      dataIndex: 'pharmacyGstin',
      key: 'pharmacyGstin',
      render: (v: string | null) => v ?? '—',
      ellipsis: true,
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 90,
      render: (_: unknown, record: CustomerInvoice) => (
        <Space size={4}>
          <Button
            type="text"
            size="small"
            icon={<EyeOutlined />}
            onClick={() => setViewInvoice(record)}
          />
          <Popconfirm
            title="Delete this invoice?"
            onConfirm={() => handleDelete(record.id)}
            okText="Delete"
            okType="danger"
          >
            <Button type="text" size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            Customer Invoices
          </Title>
        </Col>
        <Col>
          <Button type="primary" icon={<PlusOutlined />} onClick={() => setModalOpen(true)}>
            New Invoice
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Invoices</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{filtered.length}</div>
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Paid</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#3f8600' }}>{paidCount}</div>
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total GST Collected</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#1677ff' }}>
              {totalGst.toLocaleString('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 })}
            </div>
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Net Revenue</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#3f8600' }}>
              {totalNet.toLocaleString('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 })}
            </div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <Input
            placeholder="Search by invoice number, billed by or payment mode…"
            prefix={<SearchOutlined />}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onPressEnter={() => setAppliedSearch(search)}
            allowClear
            onClear={() => { setSearch(''); setAppliedSearch(''); }}
          />
        </Col>
        <Col style={{ width: 180 }}>
          <Select
            placeholder="All Statuses"
            options={STATUS_OPTIONS}
            value={statusFilter}
            onChange={setStatusFilter}
            allowClear
            style={{ width: '100%' }}
          />
        </Col>
        <Col>
          <Space>
            <Button type="primary" icon={<SearchOutlined />} onClick={() => setAppliedSearch(search)}>
              Search
            </Button>
            <Button
              icon={<ReloadOutlined />}
              onClick={() => {
                setSearch('');
                setAppliedSearch('');
                setStatusFilter(undefined);
                invoicesQuery.refetch();
              }}
            />
          </Space>
        </Col>
      </Row>

      <Table<CustomerInvoice>
        rowKey="id"
        columns={columns}
        dataSource={filtered}
        loading={invoicesQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} invoices` }}
        scroll={{ x: 1200 }}
      />

      <InvoiceFormModal open={modalOpen} onClose={() => setModalOpen(false)} />
      <InvoiceDetailDrawer invoice={viewInvoice} onClose={() => setViewInvoice(null)} />
    </div>
  );
}
