import { useState } from 'react';
import {
  Table, Tag, Space, Typography, Row, Col, Card,
  Button, Select, Tooltip, Popconfirm, message,
} from 'antd';
import { ReloadOutlined, DeleteOutlined, BellOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { useNotifications, useDeleteNotification, type AppNotification } from '@/hooks/useNotifications';

const { Title } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Pending: 'blue', Sent: 'green', Failed: 'red', Cancelled: 'default',
};
const TYPE_COLORS: Record<string, string> = {
  ReorderAlert: 'orange', ExpiryAlert: 'gold', DebtReminder: 'purple',
  SystemAlert: 'geekblue', General: 'default',
};

const TYPE_OPTIONS = Object.keys(TYPE_COLORS).map((k) => ({ value: k, label: k }));
const STATUS_OPTIONS = Object.keys(STATUS_COLORS).map((k) => ({ value: k, label: k }));

export default function NotificationsPage() {
  const [typeFilter, setTypeFilter] = useState<string | undefined>();
  const [statusFilter, setStatusFilter] = useState<string | undefined>();

  const notifQuery = useNotifications({ notificationType: typeFilter, status: statusFilter });
  const deleteNotif = useDeleteNotification();

  const notifications = notifQuery.data ?? [];
  const pendingCount = notifications.filter((n) => n.status === 'Pending').length;
  const failedCount = notifications.filter((n) => n.status === 'Failed').length;

  const handleDelete = async (id: string) => {
    try { await deleteNotif.mutateAsync(id); message.success('Notification dismissed.'); }
    catch { message.error('Failed to dismiss.'); }
  };

  const columns: ColumnsType<AppNotification> = [
    {
      title: 'Type',
      dataIndex: 'notificationType',
      width: 140,
      render: (v: string | null) => (
        <Tag color={TYPE_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>
      ),
    },
    {
      title: 'Subject',
      dataIndex: 'subject',
      ellipsis: true,
      render: (v: string | null, rec: AppNotification) => (
        <Tooltip title={rec.body ?? undefined}>
          <span>{v ?? '—'}</span>
        </Tooltip>
      ),
    },
    {
      title: 'Status',
      dataIndex: 'status',
      width: 100,
      render: (v: string | null) => <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    {
      title: 'Channel',
      dataIndex: 'channel',
      width: 90,
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Ref Type',
      dataIndex: 'referenceType',
      width: 110,
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Scheduled',
      dataIndex: 'scheduledAt',
      width: 140,
      render: (v: string) => dayjs(v).format('DD MMM YYYY HH:mm'),
      sorter: (a, b) => dayjs(a.scheduledAt).unix() - dayjs(b.scheduledAt).unix(),
      defaultSortOrder: 'descend',
    },
    {
      title: 'Sent',
      dataIndex: 'sentAt',
      width: 140,
      render: (v: string | null) => v ? dayjs(v).format('DD MMM YYYY HH:mm') : '—',
    },
    {
      title: 'Retries',
      dataIndex: 'retryCount',
      align: 'right',
      width: 70,
      render: (v: number) => v > 0 ? <Tag color="orange">{v}</Tag> : '0',
    },
    {
      title: '',
      key: 'actions',
      width: 50,
      render: (_: unknown, record: AppNotification) => (
        <Popconfirm title="Dismiss this notification?" onConfirm={() => handleDelete(record.id)} okText="Dismiss">
          <Button type="text" size="small" danger icon={<DeleteOutlined />} />
        </Popconfirm>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Space>
            <BellOutlined style={{ fontSize: 20, color: '#1677ff' }} />
            <Title level={4} style={{ margin: 0 }}>Notifications</Title>
          </Space>
        </Col>
        <Col>
          <Space>
            <Select placeholder="Type" options={TYPE_OPTIONS} value={typeFilter}
              onChange={setTypeFilter} allowClear style={{ width: 160 }} />
            <Select placeholder="Status" options={STATUS_OPTIONS} value={statusFilter}
              onChange={setStatusFilter} allowClear style={{ width: 130 }} />
            <Button icon={<ReloadOutlined />}
              onClick={() => { setTypeFilter(undefined); setStatusFilter(undefined); notifQuery.refetch(); }} />
          </Space>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{notifications.length}</div>
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Pending</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: pendingCount > 0 ? '#1677ff' : undefined }}>{pendingCount}</div>
          </Card>
        </Col>
        <Col xs={12} sm={6}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Failed</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: failedCount > 0 ? '#cf1322' : undefined }}>{failedCount}</div>
          </Card>
        </Col>
      </Row>

      <Table<AppNotification>
        rowKey="id"
        columns={columns}
        dataSource={notifications}
        loading={notifQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} notifications` }}
        scroll={{ x: 950 }}
      />
    </div>
  );
}
