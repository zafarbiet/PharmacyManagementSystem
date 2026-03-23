import { Layout, Select, Space, Avatar, Typography, Dropdown } from 'antd';
import { UserOutlined, LogoutOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useGlobalStore } from '@/store/globalStore';
import type { MenuProps } from 'antd';

const { Header } = Layout;
const { Text } = Typography;

export default function Topbar() {
  const { currentUser, selectedBranch, availableBranches, setSelectedBranch, clearAuth } =
    useGlobalStore();
  const navigate = useNavigate();

  const branchOptions = availableBranches.map((b) => ({
    value: b.id ?? '',
    label: b.name ?? 'Unnamed Branch',
  }));

  const handleBranchChange = (value: string) => {
    const branch = availableBranches.find((b) => b.id === value);
    if (branch) setSelectedBranch(branch);
  };

  const userMenuItems: MenuProps['items'] = [
    {
      key: 'logout',
      icon: <LogoutOutlined />,
      label: 'Logout',
      onClick: () => {
        clearAuth();
        navigate('/login');
      },
    },
  ];

  return (
    <Header
      style={{
        background: '#fff',
        padding: '0 24px',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        borderBottom: '1px solid #f0f0f0',
        height: 56,
      }}
    >
      <div />
      <Space size={16}>
        {branchOptions.length > 0 && (
          <Select
            style={{ width: 200 }}
            placeholder="Select branch"
            value={selectedBranch?.id}
            options={branchOptions}
            onChange={handleBranchChange}
            size="small"
          />
        )}
        <Dropdown menu={{ items: userMenuItems }} placement="bottomRight">
          <Space style={{ cursor: 'pointer' }}>
            <Avatar size="small" icon={<UserOutlined />} />
            <Text style={{ fontSize: 13 }}>{currentUser?.fullName ?? 'User'}</Text>
          </Space>
        </Dropdown>
      </Space>
    </Header>
  );
}
