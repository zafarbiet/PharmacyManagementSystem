import { Form, Input, Button, Card, Typography } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useGlobalStore } from '@/store/globalStore';

const { Title } = Typography;

interface LoginFormValues {
  username: string;
  password: string;
}

export default function LoginPage() {
  const [form] = Form.useForm<LoginFormValues>();
  const navigate = useNavigate();
  const { setCurrentUser, setAuthToken } = useGlobalStore();

  const handleSubmit = (values: LoginFormValues) => {
    // Phase 1 skeleton: accepts any non-empty credentials.
    // Replace with POST /api/auth/login in Phase 2.
    setAuthToken('pms-dev-token-placeholder');
    setCurrentUser({
      id: '00000000-0000-0000-0000-000000000001',
      username: values.username,
      fullName: values.username,
      email: null,
      phone: null,
      passwordHash: null,
      isLocked: false,
      lastLoginAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
      updatedBy: null,
      isActive: true,
    });
    navigate('/');
  };

  return (
    <div
      style={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: '#f0f2f5',
      }}
    >
      <Card style={{ width: 380, boxShadow: '0 4px 24px rgba(0,0,0,0.08)' }}>
        <Title level={3} style={{ textAlign: 'center', marginBottom: 24 }}>
          Pharmacy MS
        </Title>
        <Form form={form} onFinish={handleSubmit} layout="vertical" size="large">
          <Form.Item
            name="username"
            rules={[{ required: true, message: 'Please enter your username' }]}
          >
            <Input prefix={<UserOutlined />} placeholder="Username" autoComplete="username" />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Please enter your password' }]}
          >
            <Input.Password
              prefix={<LockOutlined />}
              placeholder="Password"
              autoComplete="current-password"
            />
          </Form.Item>
          <Form.Item style={{ marginBottom: 0 }}>
            <Button type="primary" htmlType="submit" block>
              Log In
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
}
