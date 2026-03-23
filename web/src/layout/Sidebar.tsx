import { useState } from 'react';
import { Layout, Menu } from 'antd';
import {
  MedicineBoxOutlined,
  ShoppingCartOutlined,
  FileTextOutlined,
  WarningOutlined,
  TeamOutlined,
  BarChartOutlined,
} from '@ant-design/icons';
import { useNavigate, useLocation } from 'react-router-dom';

const { Sider } = Layout;

const menuItems = [
  {
    key: '/inventory/drugs',
    icon: <MedicineBoxOutlined />,
    label: 'Drug Inventory',
  },
  {
    key: '/purchase-orders',
    icon: <ShoppingCartOutlined />,
    label: 'Purchase Orders',
    disabled: true,
  },
  {
    key: '/invoices',
    icon: <FileTextOutlined />,
    label: 'Invoices',
    disabled: true,
  },
  {
    key: '/expired-drugs',
    icon: <WarningOutlined />,
    label: 'Expiry Management',
    disabled: true,
  },
  {
    key: '/vendors',
    icon: <TeamOutlined />,
    label: 'Vendors',
    disabled: true,
  },
  {
    key: '/reports',
    icon: <BarChartOutlined />,
    label: 'Reports',
    disabled: true,
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
