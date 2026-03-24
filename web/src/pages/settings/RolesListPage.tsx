import { useState } from 'react';
import {
  Table, Button, Space, Typography, Row, Col,
  Popconfirm, message, Drawer, Tag, Select, Divider, Tooltip,
} from 'antd';
import {
  PlusOutlined, EditOutlined, DeleteOutlined, TeamOutlined, UserAddOutlined,
} from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { Role, UserRole } from '@/api/localTypes';
import type { AppUser } from '@/api/types.gen';
import { useRoles } from '@/hooks/useRoles';
import { useCreateRole, useUpdateRole, useDeleteRole } from '@/hooks/useRoleMutations';
import { useUserRoles } from '@/hooks/useUserRoles';
import { useAssignRole, useRevokeRole } from '@/hooks/useUserRoleMutations';
import { useAppUsers } from '@/hooks/useAppUsers';
import RoleFormModal from '@/components/RoleFormModal';

const { Title, Text } = Typography;

function RoleUsersDrawer({
  role,
  allUsers,
  onClose,
}: {
  role: Role | null;
  allUsers: AppUser[];
  onClose: () => void;
}) {
  const [selectedUserId, setSelectedUserId] = useState<string | undefined>();
  const userRolesQuery = useUserRoles(undefined, role?.id);
  const assignRole = useAssignRole();
  const revokeRole = useRevokeRole();

  const assignments: UserRole[] = userRolesQuery.data ?? [];

  const assignedUserIds = new Set(assignments.map((a) => a.userId));
  const unassignedUsers = allUsers.filter((u) => u.id && !assignedUserIds.has(u.id));

  const userMap: Record<string, string> = {};
  allUsers.forEach((u) => { if (u.id) userMap[u.id] = u.fullName ?? u.username ?? u.id; });

  const handleAssign = async () => {
    if (!role || !selectedUserId) return;
    try {
      await assignRole.mutateAsync({
        userId: selectedUserId,
        roleId: role.id,
        assignedAt: new Date().toISOString(),
      });
      message.success('Role assigned.');
      setSelectedUserId(undefined);
    } catch {
      message.error('Failed to assign role.');
    }
  };

  const cols: ColumnsType<UserRole> = [
    {
      title: 'User',
      dataIndex: 'userId',
      render: (id: string) => userMap[id] ?? id,
    },
    {
      title: 'Assigned At',
      dataIndex: 'assignedAt',
      render: (v: string) => dayjs(v).format('DD MMM YYYY'),
      width: 130,
    },
    {
      title: '',
      key: 'revoke',
      width: 80,
      render: (_: unknown, row: UserRole) => (
        <Popconfirm
          title="Remove this user from the role?"
          onConfirm={async () => {
            try {
              await revokeRole.mutateAsync(row.id);
              message.success('Role revoked.');
            } catch {
              message.error('Failed to revoke role.');
            }
          }}
          okText="Remove"
          okType="danger"
        >
          <Button type="text" size="small" danger icon={<DeleteOutlined />}>
            Revoke
          </Button>
        </Popconfirm>
      ),
    },
  ];

  return (
    <Drawer
      title={
        <span>
          <TeamOutlined style={{ marginRight: 8 }} />
          Users in role: <Tag color="blue">{role?.name}</Tag>
        </span>
      }
      open={!!role}
      onClose={onClose}
      width={500}
    >
      <Divider orientation="left" style={{ marginTop: 0 }}>Assign User</Divider>
      <Space.Compact style={{ width: '100%', marginBottom: 16 }}>
        <Select
          showSearch
          placeholder="Select user to assign"
          style={{ flex: 1 }}
          value={selectedUserId}
          onChange={setSelectedUserId}
          filterOption={(input, option) =>
            (option?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
          }
          options={unassignedUsers.map((u) => ({
            value: u.id,
            label: u.fullName ?? u.username ?? u.id,
          }))}
          notFoundContent={unassignedUsers.length === 0 ? 'All users already assigned' : 'No users found'}
        />
        <Button
          type="primary"
          icon={<UserAddOutlined />}
          onClick={handleAssign}
          disabled={!selectedUserId}
          loading={assignRole.isPending}
        >
          Assign
        </Button>
      </Space.Compact>

      <Divider orientation="left">Current Members ({assignments.length})</Divider>
      <Table
        dataSource={assignments}
        columns={cols}
        rowKey="id"
        size="small"
        pagination={false}
        loading={userRolesQuery.isFetching}
        locale={{ emptyText: <Text type="secondary">No users assigned yet</Text> }}
      />
    </Drawer>
  );
}

export default function RolesListPage() {
  const [modalState, setModalState] = useState<{ open: boolean; role: Role | null }>({
    open: false,
    role: null,
  });
  const [drawerRole, setDrawerRole] = useState<Role | null>(null);

  const rolesQuery = useRoles();
  const usersQuery = useAppUsers();
  const createRole = useCreateRole();
  const updateRole = useUpdateRole();
  const deleteRole = useDeleteRole();

  const roles: Role[] = rolesQuery.data ?? [];
  const allUsers: AppUser[] = (usersQuery.data ?? []) as AppUser[];

  const handleSubmit = async (values: Partial<Role>) => {
    try {
      if (modalState.role) {
        await updateRole.mutateAsync({ id: modalState.role.id, ...values });
        message.success('Role updated.');
      } else {
        await createRole.mutateAsync(values);
        message.success('Role created.');
      }
      setModalState({ open: false, role: null });
    } catch {
      message.error('Failed to save role.');
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteRole.mutateAsync(id);
      message.success('Role deleted.');
    } catch {
      message.error('Failed to delete role.');
    }
  };

  const columns: ColumnsType<Role> = [
    {
      title: 'Name',
      dataIndex: 'name',
      render: (v: string | null) => v ? <Tag color="blue">{v}</Tag> : '—',
      sorter: (a, b) => (a.name ?? '').localeCompare(b.name ?? ''),
    },
    {
      title: 'Description',
      dataIndex: 'description',
      render: (v: string | null) => v ?? <span style={{ color: '#bbb' }}>—</span>,
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 140,
      render: (_: unknown, record: Role) => (
        <Space size={4}>
          <Tooltip title="Manage Users">
            <Button
              size="small"
              icon={<TeamOutlined />}
              onClick={() => setDrawerRole(record)}
            />
          </Tooltip>
          <Button
            size="small"
            icon={<EditOutlined />}
            onClick={() => setModalState({ open: true, role: record })}
          />
          <Popconfirm
            title="Delete this role?"
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
          <Title level={4} style={{ margin: 0 }}>Roles</Title>
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => setModalState({ open: true, role: null })}
          >
            New Role
          </Button>
        </Col>
      </Row>

      <Table<Role>
        rowKey="id"
        columns={columns}
        dataSource={roles}
        loading={rolesQuery.isFetching}
        size="small"
        pagination={{ pageSize: 25, showSizeChanger: true, showTotal: (t) => `${t} roles` }}
      />

      <RoleFormModal
        open={modalState.open}
        role={modalState.role}
        onClose={() => setModalState({ open: false, role: null })}
        onSubmit={handleSubmit}
        loading={createRole.isPending || updateRole.isPending}
      />

      <RoleUsersDrawer
        role={drawerRole}
        allUsers={allUsers}
        onClose={() => setDrawerRole(null)}
      />
    </div>
  );
}
