import { useState } from 'react';
import { Card, Select, Table, Checkbox, Typography, Spin, Alert, Space, Tag } from 'antd';
import { useRoles } from '@/hooks/useRoles';
import { useMenuItems } from '@/hooks/useMenuItems';
import { useRoleMenuItems } from '@/hooks/useRoleMenuItems';
import { useCreateRoleMenuItem, useDeleteRoleMenuItem } from '@/hooks/useRoleMenuItemMutations';
import type { ColumnsType } from 'antd/es/table';
import type { MenuItem } from '@/hooks/useMenuItems';

const { Title, Text } = Typography;

export default function MenuConfigPage() {
  const [selectedRoleId, setSelectedRoleId] = useState<string | null>(null);

  const { data: roles, isLoading: rolesLoading } = useRoles();
  const { data: menuItems, isLoading: menuItemsLoading } = useMenuItems();
  const { data: roleMenuItems, isLoading: roleMenuItemsLoading } = useRoleMenuItems(
    selectedRoleId ? { roleId: selectedRoleId } : {},
  );

  const createMutation = useCreateRoleMenuItem();
  const deleteMutation = useDeleteRoleMenuItem();

  const assignedMenuItemIds = new Set(roleMenuItems?.map((rmi) => rmi.menuItemId) ?? []);
  const hasAnyAssignments = (roleMenuItems?.length ?? 0) > 0;

  const handleToggle = async (menuItem: MenuItem, checked: boolean) => {
    if (!selectedRoleId) return;

    if (checked) {
      await createMutation.mutateAsync({ roleId: selectedRoleId, menuItemId: menuItem.id });
    } else {
      const existing = roleMenuItems?.find((rmi) => rmi.menuItemId === menuItem.id);
      if (existing) {
        await deleteMutation.mutateAsync(existing.id);
      }
    }
  };

  const topLevel = menuItems?.filter((m) => !m.parentKey) ?? [];
  const children = (parentKey: string) => menuItems?.filter((m) => m.parentKey === parentKey) ?? [];

  const flatRows: (MenuItem & { isGroup: boolean; depth: number })[] = [];
  for (const item of topLevel) {
    flatRows.push({ ...item, isGroup: false, depth: 0 });
    const subs = children(item.key ?? '');
    for (const sub of subs) {
      flatRows.push({ ...sub, isGroup: false, depth: 1 });
    }
  }

  const columns: ColumnsType<(typeof flatRows)[0]> = [
    {
      title: 'Menu Item',
      key: 'label',
      render: (_, row) => (
        <Space>
          <span style={{ paddingLeft: row.depth * 24 }}>{row.label}</span>
          {row.depth === 0 && children(row.key ?? '').length > 0 && (
            <Tag color="blue">Group</Tag>
          )}
        </Space>
      ),
    },
    {
      title: 'Route Key',
      dataIndex: 'key',
      key: 'key',
      render: (val: string) => <Text code>{val}</Text>,
    },
    {
      title: 'Icon',
      dataIndex: 'icon',
      key: 'icon',
      render: (val: string | null) => val ? <Text type="secondary">{val}</Text> : '—',
    },
    {
      title: () => (
        <Space direction="vertical" size={0}>
          <span>Allowed</span>
          {!selectedRoleId && <Text type="secondary" style={{ fontSize: 11 }}>Select a role</Text>}
          {selectedRoleId && !hasAnyAssignments && (
            <Text type="secondary" style={{ fontSize: 11 }}>All shown (no restrictions)</Text>
          )}
        </Space>
      ),
      key: 'allowed',
      width: 140,
      render: (_, row) => {
        if (!selectedRoleId) return null;
        const isChecked = assignedMenuItemIds.has(row.id);
        const isLoading =
          (createMutation.isPending || deleteMutation.isPending) &&
          (createMutation.variables?.menuItemId === row.id ||
            deleteMutation.variables === roleMenuItems?.find((r) => r.menuItemId === row.id)?.id);
        return (
          <Checkbox
            checked={isChecked}
            disabled={isLoading}
            onChange={(e) => handleToggle(row, e.target.checked)}
          />
        );
      },
    },
  ];

  const isLoading = rolesLoading || menuItemsLoading;

  return (
    <div style={{ padding: 24 }}>
      <Title level={3}>Menu Configuration</Title>
      <Alert
        type="info"
        showIcon
        style={{ marginBottom: 16 }}
        message="Configure which menu items each role can access."
        description="If no items are assigned to any role, all users see the full menu. Once you assign items to a role, only those items will be visible to users with that role."
      />

      <Card>
        <Space direction="vertical" size={16} style={{ width: '100%' }}>
          <div>
            <Text strong>Select Role: </Text>
            <Select
              placeholder="Choose a role to configure"
              style={{ width: 280, marginLeft: 8 }}
              loading={rolesLoading}
              allowClear
              onChange={(val) => setSelectedRoleId(val ?? null)}
              options={roles?.map((r) => ({ label: r.name, value: r.id }))}
            />
          </div>

          {isLoading ? (
            <Spin />
          ) : (
            <Table
              dataSource={flatRows}
              columns={columns}
              rowKey="id"
              loading={roleMenuItemsLoading}
              pagination={false}
              size="small"
              rowClassName={(row) =>
                row.depth === 0 && children(row.key ?? '').length > 0 ? 'ant-table-row-level-0' : ''
              }
            />
          )}
        </Space>
      </Card>
    </div>
  );
}
