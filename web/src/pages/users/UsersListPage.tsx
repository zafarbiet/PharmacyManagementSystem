import { useState } from 'react';
import { Table, Button, Tag, Space, Typography, Row, Col, Card, Popconfirm, message, Input } from 'antd';
import { PlusOutlined, EditOutlined, UserDeleteOutlined, ReloadOutlined, SearchOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { AppUser } from '@/api/types.gen';
import { useAppUsers } from '@/hooks/useAppUsers';
import { useDeleteAppUser } from '@/hooks/useAppUserMutations';
import UserFormModal from '@/components/UserFormModal';
import { useGlobalStore } from '@/store/globalStore';

const { Title } = Typography;

export default function UsersListPage() {
  const [search, setSearch] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingUser, setEditingUser] = useState<AppUser | null>(null);

  const usersQuery = useAppUsers();
  const deleteUser = useDeleteAppUser();
  const currentUser = useGlobalStore((s) => s.currentUser);

  const users = usersQuery.data ?? [];

  const filtered = search
    ? users.filter(
        (u) =>
          (u.username ?? '').toLowerCase().includes(search.toLowerCase()) ||
          (u.fullName ?? '').toLowerCase().includes(search.toLowerCase()) ||
          (u.email ?? '').toLowerCase().includes(search.toLowerCase()),
      )
    : users;

  const activeCount = users.filter((u) => u.isActive).length;

  const handleDelete = async (id: string) => {
    try {
      await deleteUser.mutateAsync(id);
      message.success('User deactivated.');
    } catch {
      message.error('Failed to deactivate user.');
    }
  };

  const columns: ColumnsType<AppUser> = [
    {
      title: 'Username',
      dataIndex: 'username',
      key: 'username',
      render: (v: string | null) => <strong>{v ?? '—'}</strong>,
      sorter: (a, b) => (a.username ?? '').localeCompare(b.username ?? ''),
    },
    {
      title: 'Full Name',
      dataIndex: 'fullName',
      key: 'fullName',
      sorter: (a, b) => (a.fullName ?? '').localeCompare(b.fullName ?? ''),
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Phone',
      dataIndex: 'phone',
      key: 'phone',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Last Login',
      dataIndex: 'lastLoginAt',
      key: 'lastLoginAt',
      render: (v: string | null) => (v ? dayjs(v).format('DD MMM YYYY HH:mm') : '—'),
      sorter: (a, b) => dayjs(a.lastLoginAt ?? 0).unix() - dayjs(b.lastLoginAt ?? 0).unix(),
    },
    {
      title: 'Status',
      dataIndex: 'isActive',
      key: 'isActive',
      render: (v: boolean) => <Tag color={v ? 'green' : 'default'}>{v ? 'Active' : 'Inactive'}</Tag>,
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 90,
      render: (_: unknown, record: AppUser) => {
        const isSelf = record.id === currentUser?.id;
        return (
          <Space size={4}>
            <Button
              type="text"
              size="small"
              icon={<EditOutlined />}
              onClick={() => { setEditingUser(record); setModalOpen(true); }}
            />
            <Popconfirm
              title="Deactivate this user?"
              description={isSelf ? 'You cannot deactivate yourself.' : undefined}
              onConfirm={() => !isSelf && handleDelete(record.id!)}
              okText="Deactivate"
              okType="danger"
              disabled={isSelf}
            >
              <Button
                type="text"
                size="small"
                danger
                icon={<UserDeleteOutlined />}
                disabled={isSelf}
              />
            </Popconfirm>
          </Space>
        );
      },
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>User Management</Title>
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => { setEditingUser(null); setModalOpen(true); }}
          >
            Add User
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Users</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{users.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Active</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#3f8600' }}>{activeCount}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Inactive</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#888' }}>{users.length - activeCount}</div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <Input
            placeholder="Search by username, name or email…"
            prefix={<SearchOutlined />}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            allowClear
            onClear={() => setSearch('')}
          />
        </Col>
        <Col>
          <Button
            icon={<ReloadOutlined />}
            onClick={() => { setSearch(''); usersQuery.refetch(); }}
          />
        </Col>
      </Row>

      <Table<AppUser>
        rowKey="id"
        columns={columns}
        dataSource={filtered}
        loading={usersQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} users` }}
        scroll={{ x: 800 }}
      />

      <UserFormModal
        open={modalOpen}
        user={editingUser}
        onClose={() => setModalOpen(false)}
      />
    </div>
  );
}
