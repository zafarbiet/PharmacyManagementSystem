import { useState, useMemo } from 'react';
import { Table, Input, Select, Button, Tag, Space, Typography, Row, Col, Card, Popconfirm, Tooltip, message } from 'antd';
import {
  SearchOutlined, PlusOutlined, ReloadOutlined,
  CheckCircleOutlined, CloseCircleOutlined, DeleteOutlined,
} from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { PurchaseOrder } from '@/api/types.gen';
import { usePurchaseOrders } from '@/hooks/usePurchaseOrders';
import { useVendors } from '@/hooks/useVendors';
import { useDeletePurchaseOrder, useApprovePurchaseOrder, useRejectPurchaseOrder } from '@/hooks/usePurchaseOrderMutations';
import PurchaseOrderFormModal from '@/components/PurchaseOrderFormModal';
import { useGlobalStore } from '@/store/globalStore';

const { Title } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Draft: 'default',
  Pending: 'blue',
  Approved: 'cyan',
  Ordered: 'geekblue',
  PartiallyReceived: 'orange',
  Received: 'green',
  Cancelled: 'red',
  Rejected: 'volcano',
};

const STATUS_OPTIONS = Object.keys(STATUS_COLORS).map((s) => ({ value: s, label: s }));

const APPROVABLE = ['Draft', 'Pending'];
const REJECTABLE = ['Draft', 'Pending', 'Approved'];

export default function PurchaseOrdersListPage() {
  const [search, setSearch] = useState('');
  const [appliedSearch, setAppliedSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState<string | undefined>();
  const [modalOpen, setModalOpen] = useState(false);

  const currentUser = useGlobalStore((s) => s.currentUser);
  const username = currentUser?.username ?? 'system';

  const ordersQuery = usePurchaseOrders();
  const vendorsQuery = useVendors();
  const deletePO = useDeletePurchaseOrder();
  const approvePO = useApprovePurchaseOrder();
  const rejectPO = useRejectPurchaseOrder();

  const vendorMap = useMemo(() => {
    const map = new Map<string, string>();
    for (const v of vendorsQuery.data ?? []) {
      if (v.id) map.set(v.id, v.name ?? v.id);
    }
    return map;
  }, [vendorsQuery.data]);

  const filtered = useMemo(() => {
    let rows = ordersQuery.data ?? [];
    if (appliedSearch) {
      const lower = appliedSearch.toLowerCase();
      rows = rows.filter(
        (o) =>
          (o.poNumber ?? '').toLowerCase().includes(lower) ||
          vendorMap.get(o.vendorId ?? '')?.toLowerCase().includes(lower),
      );
    }
    if (statusFilter) rows = rows.filter((o) => o.status === statusFilter);
    return rows;
  }, [ordersQuery.data, appliedSearch, statusFilter, vendorMap]);

  const handleApprove = async (id: string) => {
    try {
      await approvePO.mutateAsync({ id, approvedBy: username });
      message.success('Purchase order approved.');
    } catch {
      message.error('Failed to approve PO.');
    }
  };

  const handleReject = async (id: string) => {
    try {
      await rejectPO.mutateAsync({ id, rejectedBy: username });
      message.success('Purchase order rejected.');
    } catch {
      message.error('Failed to reject PO.');
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deletePO.mutateAsync(id);
      message.success('Purchase order deleted.');
    } catch {
      message.error('Failed to delete PO.');
    }
  };

  const totalAmount = filtered.reduce((s, o) => s + (o.totalAmount ?? 0), 0);
  const pendingCount = filtered.filter((o) => o.status === 'Pending' || o.status === 'Draft').length;

  const columns: ColumnsType<PurchaseOrder> = [
    {
      title: 'PO Number',
      dataIndex: 'poNumber',
      key: 'poNumber',
      render: (v: string | null) => <strong>{v ?? '—'}</strong>,
      sorter: (a, b) => (a.poNumber ?? '').localeCompare(b.poNumber ?? ''),
    },
    {
      title: 'Vendor',
      dataIndex: 'vendorId',
      key: 'vendorId',
      render: (id: string) => vendorMap.get(id) ?? id,
      sorter: (a, b) =>
        (vendorMap.get(a.vendorId ?? '') ?? '').localeCompare(vendorMap.get(b.vendorId ?? '') ?? ''),
    },
    {
      title: 'Order Date',
      dataIndex: 'orderDate',
      key: 'orderDate',
      render: (v: string) => (v ? dayjs(v).format('DD MMM YYYY') : '—'),
      sorter: (a, b) => dayjs(a.orderDate).unix() - dayjs(b.orderDate).unix(),
      defaultSortOrder: 'descend',
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: (v: string | null) => <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    {
      title: 'Total Amount',
      dataIndex: 'totalAmount',
      key: 'totalAmount',
      align: 'right',
      render: (v: number) => (v ?? 0).toLocaleString('en-IN', { style: 'currency', currency: 'INR' }),
      sorter: (a, b) => (a.totalAmount ?? 0) - (b.totalAmount ?? 0),
    },
    {
      title: 'Approved By',
      dataIndex: 'approvedBy',
      key: 'approvedBy',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 120,
      render: (_: unknown, record: PurchaseOrder) => (
        <Space size={4}>
          {APPROVABLE.includes(record.status ?? '') && (
            <Tooltip title="Approve">
              <Popconfirm
                title="Approve this PO?"
                onConfirm={() => handleApprove(record.id!)}
                okText="Approve"
              >
                <Button type="text" size="small" icon={<CheckCircleOutlined style={{ color: '#52c41a' }} />} />
              </Popconfirm>
            </Tooltip>
          )}
          {REJECTABLE.includes(record.status ?? '') && (
            <Tooltip title="Reject">
              <Popconfirm
                title="Reject this PO?"
                onConfirm={() => handleReject(record.id!)}
                okText="Reject"
                okType="danger"
              >
                <Button type="text" size="small" icon={<CloseCircleOutlined style={{ color: '#ff4d4f' }} />} />
              </Popconfirm>
            </Tooltip>
          )}
          <Popconfirm
            title="Delete this PO?"
            onConfirm={() => handleDelete(record.id!)}
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
          <Title level={4} style={{ margin: 0 }}>Purchase Orders</Title>
        </Col>
        <Col>
          <Button type="primary" icon={<PlusOutlined />} onClick={() => setModalOpen(true)}>
            New PO
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Orders</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{filtered.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Pending / Draft</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: pendingCount > 0 ? '#d46b08' : '#3f8600' }}>
              {pendingCount}
            </div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Value</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>
              {totalAmount.toLocaleString('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 })}
            </div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <Input
            placeholder="Search by PO number or vendor…"
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
              onClick={() => { setSearch(''); setAppliedSearch(''); setStatusFilter(undefined); ordersQuery.refetch(); }}
            />
          </Space>
        </Col>
      </Row>

      <Table<PurchaseOrder>
        rowKey="id"
        columns={columns}
        dataSource={filtered}
        loading={ordersQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} orders` }}
        scroll={{ x: 950 }}
      />

      <PurchaseOrderFormModal open={modalOpen} onClose={() => setModalOpen(false)} />
    </div>
  );
}
