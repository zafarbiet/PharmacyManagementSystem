import { useState, useMemo } from 'react';
import {
  Table, Button, Tag, Space, Typography, Row, Col, Card,
  Popconfirm, Tooltip, Select, Input, message,
} from 'antd';
import {
  PlusOutlined, CheckCircleOutlined, DeleteOutlined,
  SearchOutlined, ReloadOutlined, UnorderedListOutlined,
} from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { useSubscriptions, useSubscriptionItems, type CustomerSubscription } from '@/hooks/useSubscriptions';
import { useDeleteSubscription, useApproveSubscription } from '@/hooks/useSubscriptionMutations';
import { usePatients } from '@/hooks/usePatients';
import { useDrugs } from '@/hooks/useDrugs';
import { useGlobalStore } from '@/store/globalStore';
import SubscriptionFormModal from '@/components/SubscriptionFormModal';

const { Title } = Typography;

const APPROVAL_COLORS: Record<string, string> = {
  Pending: 'blue', Approved: 'green', Rejected: 'red',
};
const STATUS_COLORS: Record<string, string> = {
  Active: 'green', Paused: 'orange', Cancelled: 'red', Expired: 'default',
};

function SubscriptionItems({ subscriptionId }: { subscriptionId: string }) {
  const itemsQuery = useSubscriptionItems(subscriptionId);
  const drugsQuery = useDrugs();
  const drugMap = useMemo(
    () => new Map((drugsQuery.data ?? []).map((d) => [d.id ?? '', d.name ?? d.id ?? ''])),
    [drugsQuery.data],
  );
  const items = itemsQuery.data ?? [];
  if (items.length === 0) return <span style={{ color: '#888' }}>No drugs on this subscription.</span>;
  return (
    <Table
      size="small"
      rowKey="id"
      pagination={false}
      dataSource={items}
      columns={[
        { title: 'Drug', dataIndex: 'drugId', render: (id: string) => drugMap.get(id) ?? id },
        { title: 'Qty / Cycle', dataIndex: 'quantityPerCycle', align: 'right' },
      ]}
    />
  );
}

export default function SubscriptionsListPage() {
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<CustomerSubscription | null>(null);
  const [patientSearch, setPatientSearch] = useState('');
  const [approvalFilter, setApprovalFilter] = useState<string | undefined>();
  const [statusFilter, setStatusFilter] = useState<string | undefined>();

  const currentUser = useGlobalStore((s) => s.currentUser);
  const username = currentUser?.username ?? 'system';

  const subsQuery = useSubscriptions({ approvalStatus: approvalFilter, status: statusFilter });
  const patientsQuery = usePatients();
  const deleteSub = useDeleteSubscription();
  const approveSub = useApproveSubscription();

  const patientMap = useMemo(
    () => new Map((patientsQuery.data ?? []).map((p) => [p.id, p.name ?? p.id])),
    [patientsQuery.data],
  );

  const filtered = useMemo(() => {
    let rows = subsQuery.data ?? [];
    if (patientSearch) {
      const lower = patientSearch.toLowerCase();
      rows = rows.filter((s) => (patientMap.get(s.patientId) ?? '').toLowerCase().includes(lower));
    }
    return rows;
  }, [subsQuery.data, patientSearch, patientMap]);

  const pendingCount = (subsQuery.data ?? []).filter((s) => s.approvalStatus === 'Pending').length;

  const handleDelete = async (id: string) => {
    try { await deleteSub.mutateAsync(id); message.success('Subscription deleted.'); }
    catch { message.error('Failed to delete.'); }
  };

  const handleApprove = async (id: string) => {
    try { await approveSub.mutateAsync({ id, approvedBy: username }); message.success('Subscription approved.'); }
    catch { message.error('Failed to approve.'); }
  };

  const columns: ColumnsType<CustomerSubscription> = [
    {
      title: 'Patient',
      dataIndex: 'patientId',
      render: (id: string) => patientMap.get(id) ?? id,
    },
    {
      title: 'Status',
      dataIndex: 'status',
      render: (v: string | null) => <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    {
      title: 'Approval',
      dataIndex: 'approvalStatus',
      render: (v: string | null) => <Tag color={APPROVAL_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    {
      title: 'Start',
      dataIndex: 'startDate',
      render: (v: string) => dayjs(v).format('DD MMM YYYY'),
      sorter: (a, b) => dayjs(a.startDate).unix() - dayjs(b.startDate).unix(),
      defaultSortOrder: 'descend',
    },
    {
      title: 'End',
      dataIndex: 'endDate',
      render: (v: string | null) => v ? dayjs(v).format('DD MMM YYYY') : 'Ongoing',
    },
    {
      title: 'Cycle Day',
      dataIndex: 'cycleDayOfMonth',
      align: 'right',
      width: 90,
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 100,
      render: (_: unknown, record: CustomerSubscription) => (
        <Space size={4}>
          {record.approvalStatus === 'Pending' && (
            <Tooltip title="Approve">
              <Popconfirm title="Approve?" onConfirm={() => handleApprove(record.id)} okText="Approve">
                <Button type="text" size="small" icon={<CheckCircleOutlined style={{ color: '#52c41a' }} />} />
              </Popconfirm>
            </Tooltip>
          )}
          <Button type="text" size="small" icon={<UnorderedListOutlined />}
            onClick={() => { setEditing(record); setModalOpen(true); }} />
          <Popconfirm title="Delete?" onConfirm={() => handleDelete(record.id)} okText="Delete" okType="danger">
            <Button type="text" size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col><Title level={4} style={{ margin: 0 }}>Customer Subscriptions</Title></Col>
        <Col>
          <Button type="primary" icon={<PlusOutlined />} onClick={() => { setEditing(null); setModalOpen(true); }}>
            New Subscription
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{(subsQuery.data ?? []).length}</div>
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Pending Approval</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: pendingCount > 0 ? '#d46b08' : undefined }}>{pendingCount}</div>
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Active</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#3f8600' }}>
              {(subsQuery.data ?? []).filter((s) => s.status === 'Active').length}
            </div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <Input
            prefix={<SearchOutlined />}
            placeholder="Search patient name…"
            value={patientSearch}
            onChange={(e) => setPatientSearch(e.target.value)}
            allowClear
            onClear={() => setPatientSearch('')}
          />
        </Col>
        <Col style={{ width: 160 }}>
          <Select placeholder="Approval" options={Object.keys(APPROVAL_COLORS).map((k) => ({ value: k, label: k }))}
            value={approvalFilter} onChange={setApprovalFilter} allowClear style={{ width: '100%' }} />
        </Col>
        <Col style={{ width: 160 }}>
          <Select placeholder="Status" options={Object.keys(STATUS_COLORS).map((k) => ({ value: k, label: k }))}
            value={statusFilter} onChange={setStatusFilter} allowClear style={{ width: '100%' }} />
        </Col>
        <Col>
          <Button icon={<ReloadOutlined />} onClick={() => { setPatientSearch(''); setApprovalFilter(undefined); setStatusFilter(undefined); subsQuery.refetch(); }} />
        </Col>
      </Row>

      <Table<CustomerSubscription>
        rowKey="id"
        columns={columns}
        dataSource={filtered}
        loading={subsQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} subscriptions` }}
        expandable={{
          expandedRowRender: (record) => <SubscriptionItems subscriptionId={record.id} />,
        }}
      />

      <SubscriptionFormModal
        open={modalOpen}
        subscription={editing}
        onClose={() => { setModalOpen(false); setEditing(null); }}
      />
    </div>
  );
}
