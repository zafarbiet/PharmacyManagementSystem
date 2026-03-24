import { useState } from 'react';
import { Layout, Menu } from 'antd';
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
} from '@ant-design/icons';
import { useNavigate, useLocation } from 'react-router-dom';

const { Sider } = Layout;

const menuItems = [
  {
    key: '/',
    icon: <BarChartOutlined />,
    label: 'Dashboard',
  },
  {
    key: '/inventory/drugs',
    icon: <MedicineBoxOutlined />,
    label: 'Drug Inventory',
  },
  {
    key: '/drugs',
    icon: <MedicineBoxOutlined />,
    label: 'Drug Master',
  },
  {
    key: '/purchase-orders',
    icon: <ShoppingCartOutlined />,
    label: 'Purchase Orders',
  },
  {
    key: '/patients',
    icon: <ContactsOutlined />,
    label: 'Patients',
  },
  {
    key: '/invoices',
    icon: <FileTextOutlined />,
    label: 'Invoices',
  },
  {
    key: '/expired-drugs',
    icon: <WarningOutlined />,
    label: 'Expiry Management',
  },
  {
    key: '/vendors',
    icon: <TeamOutlined />,
    label: 'Vendors',
  },
  {
    key: 'quotations',
    icon: <SolutionOutlined />,
    label: 'Quotations',
    children: [
      { key: '/quotations/rfq', icon: <SendOutlined />, label: 'RFQ' },
      { key: '/quotations', icon: <FileTextOutlined />, label: 'Received' },
    ],
  },
  {
    key: '/reports',
    icon: <BarChartOutlined />,
    label: 'Reports',
  },
  {
    key: '/users',
    icon: <UserOutlined />,
    label: 'Users',
  },
  {
    key: 'settings',
    icon: <SettingOutlined />,
    label: 'Settings',
    children: [
      { key: '/settings/branches', icon: <BankOutlined />, label: 'Branches' },
      { key: '/settings/drug-categories', icon: <TagsOutlined />, label: 'Drug Categories' },
    ],
  },
];

export default function Sidebar() {
  const [collapsed, setCollapsed] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

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
      <Menu
        theme="dark"
        mode="inline"
        selectedKeys={[location.pathname]}
        items={menuItems}
        onClick={({ key }) => navigate(key)}
      />
    </Sider>
  );
}
