import { useState, useMemo } from 'react';
import { Layout, Menu, Spin } from 'antd';
import {
  MedicineBoxOutlined,
  ShoppingCartOutlined,
  FileTextOutlined,
  WarningOutlined,
  TeamOutlined,
  BarChartOutlined,
  UserOutlined,
  SettingOutlined,
  BankOutlined,
  TagsOutlined,
  SolutionOutlined,
  SendOutlined,
  ContactsOutlined,
  CreditCardOutlined,
  AlertOutlined,
  BellOutlined,
  ExperimentOutlined,
  InboxOutlined,
  MenuOutlined,
  DashboardOutlined,
  QuestionCircleOutlined,
} from '@ant-design/icons';
import { useNavigate, useLocation } from 'react-router-dom';
import { useGlobalStore } from '@/store/globalStore';
import { useMenuItemsForUser } from '@/hooks/useMenuItems';
import type { MenuItem } from '@/hooks/useMenuItems';

const { Sider } = Layout;

const iconMap: Record<string, React.ReactNode> = {
  BarChartOutlined: <BarChartOutlined />,
  DashboardOutlined: <DashboardOutlined />,
  MedicineBoxOutlined: <MedicineBoxOutlined />,
  ShoppingCartOutlined: <ShoppingCartOutlined />,
  FileTextOutlined: <FileTextOutlined />,
  WarningOutlined: <WarningOutlined />,
  TeamOutlined: <TeamOutlined />,
  UserOutlined: <UserOutlined />,
  SettingOutlined: <SettingOutlined />,
  BankOutlined: <BankOutlined />,
  TagsOutlined: <TagsOutlined />,
  SolutionOutlined: <SolutionOutlined />,
  SendOutlined: <SendOutlined />,
  ContactsOutlined: <ContactsOutlined />,
  CreditCardOutlined: <CreditCardOutlined />,
  AlertOutlined: <AlertOutlined />,
  BellOutlined: <BellOutlined />,
  ExperimentOutlined: <ExperimentOutlined />,
  InboxOutlined: <InboxOutlined />,
  MenuOutlined: <MenuOutlined />,
};

function buildMenuItems(items: MenuItem[]) {
  const topLevel = items.filter((m) => !m.parentKey);
  return topLevel.map((item) => {
    const icon = item.icon ? (iconMap[item.icon] ?? <QuestionCircleOutlined />) : undefined;
    const childItems = items.filter((c) => c.parentKey === item.key);

    if (childItems.length > 0) {
      return {
        key: item.key!,
        icon,
        label: item.label,
        children: childItems.map((child) => ({
          key: child.key!,
          icon: child.icon ? (iconMap[child.icon] ?? <QuestionCircleOutlined />) : undefined,
          label: child.label,
        })),
      };
    }

    return { key: item.key!, icon, label: item.label };
  });
}

export default function Sidebar() {
  const [collapsed, setCollapsed] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const currentUser = useGlobalStore((s) => s.currentUser);

  const { data: menuItems, isLoading } = useMenuItemsForUser(currentUser?.username);

  const antMenuItems = useMemo(
    () => (menuItems ? buildMenuItems(menuItems) : []),
    [menuItems],
  );

  return (
    <Sider
      collapsible
      collapsed={collapsed}
      onCollapse={setCollapsed}
      style={{ background: '#001529' }}
      width={220}
    >
      <div
        style={{
          height: 48,
          margin: 16,
          display: 'flex',
          alignItems: 'center',
          color: '#fff',
          fontWeight: 700,
          fontSize: collapsed ? 16 : 14,
          letterSpacing: 1,
          overflow: 'hidden',
          whiteSpace: 'nowrap',
        }}
      >
        {collapsed ? 'PMS' : 'Pharmacy MS'}
      </div>
      {isLoading ? (
        <div style={{ display: 'flex', justifyContent: 'center', paddingTop: 24 }}>
          <Spin size="small" />
        </div>
      ) : (
        <Menu
          theme="dark"
          mode="inline"
          selectedKeys={[location.pathname]}
          items={antMenuItems}
          onClick={({ key }) => navigate(key)}
        />
      )}
    </Sider>
  );
}
