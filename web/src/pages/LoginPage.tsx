import { useState } from 'react';
import { Form, Input, Button, Card, Typography, Alert } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import axiosClient from '@/api/axiosClient';
import { useGlobalStore } from '@/store/globalStore';
import type { LoginRequest, LoginResponse } from '@/api/localTypes';

const { Title } = Typography;

export default function LoginPage() {
  const [form] = Form.useForm<LoginRequest>();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const { setCurrentUser, setAuthToken } = useGlobalStore();

  const handleSubmit = async (values: LoginRequest) => {
    setLoading(true);
    setError(null);
    try {
      const { data } = await axiosClient.post<LoginResponse>('/auth/login', values);
      setAuthToken(data.token);
      if (data.user) setCurrentUser(data.user);
      navigate('/');
    } catch (err: unknown) {
      const status = (err as { response?: { status?: number } })?.response?.status;
      if (status === 401) {
        setError('Invalid username or password.');
      } else if (status === 422) {
        setError('Please enter both username and password.');
      } else {
        setError('Login failed. Please try again.');
      }
    } finally {
      setLoading(false);
    }
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
        {error && (
          <Alert type="error" message={error} showIcon style={{ marginBottom: 16 }} />
        )}
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
            <Button type="primary" htmlType="submit" block loading={loading}>
              Log In
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
}
