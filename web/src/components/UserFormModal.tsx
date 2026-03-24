import { useEffect } from 'react';
import { Modal, Form, Input, Row, Col, message, Typography } from 'antd';
import type { AppUser } from '@/api/types.gen';
import { useCreateAppUser, useUpdateAppUser } from '@/hooks/useAppUserMutations';

const { Text } = Typography;

interface Props {
  open: boolean;
  user?: AppUser | null;
  onClose: () => void;
}

type FormValues = {
  username: string;
  fullName: string;
  email?: string;
  phone?: string;
  password?: string;
};

async function hashPassword(username: string, password: string): Promise<string> {
  const data = new TextEncoder().encode(`${username}:${password}`);
  const buf = await crypto.subtle.digest('SHA-256', data);
  return Array.from(new Uint8Array(buf))
    .map((b) => b.toString(16).padStart(2, '0'))
    .join('')
    .toUpperCase();
}

export default function UserFormModal({ open, user, onClose }: Props) {
  const [form] = Form.useForm<FormValues>();
  const isEdit = !!user?.id;

  const create = useCreateAppUser();
  const update = useUpdateAppUser();
  const isPending = create.isPending || update.isPending;

  useEffect(() => {
    if (open) {
      form.setFieldsValue(
        user
          ? {
              username: user.username ?? '',
              fullName: user.fullName ?? '',
              email: user.email ?? '',
              phone: user.phone ?? '',
              password: '',
            }
          : { username: '', fullName: '', email: '', phone: '', password: '' },
      );
    }
  }, [open, user, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    try {
      const username = values.username.trim();

      if (isEdit && user?.id) {
        let passwordHash = user.passwordHash ?? '';
        if (values.password) {
          passwordHash = await hashPassword(username, values.password);
        }
        await update.mutateAsync({
          id: user.id,
          username,
          fullName: values.fullName,
          email: values.email || null,
          phone: values.phone || null,
          passwordHash,
        });
        message.success('User updated.');
      } else {
        if (!values.password) {
          message.error('Password is required for new users.');
          return;
        }
        const passwordHash = await hashPassword(username, values.password);
        await create.mutateAsync({
          username,
          fullName: values.fullName,
          email: values.email || null,
          phone: values.phone || null,
          passwordHash,
        });
        message.success('User created.');
      }

      form.resetFields();
      onClose();
    } catch {
      message.error('Failed to save user.');
    }
  };

  return (
    <Modal
      title={isEdit ? 'Edit User' : 'Add User'}
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); onClose(); }}
      okText={isEdit ? 'Save Changes' : 'Add User'}
      confirmLoading={isPending}
      width={560}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Row gutter={16}>
          <Col span={12}>
            <Form.Item
              name="username"
              label="Username"
              rules={[{ required: true, message: 'Required' }, { min: 3 }]}
            >
              <Input placeholder="e.g. pharmacist2" disabled={isEdit} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              name="fullName"
              label="Full Name"
              rules={[{ required: true, message: 'Required' }]}
            >
              <Input placeholder="e.g. Rahul Sharma" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="email" label="Email">
              <Input type="email" placeholder="e.g. rahul@pharmacy.in" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="phone" label="Phone">
              <Input placeholder="e.g. 9876543210" maxLength={15} />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item
          name="password"
          label="Password"
          rules={isEdit ? [] : [{ required: true, message: 'Required' }]}
        >
          <Input.Password placeholder={isEdit ? 'Leave blank to keep current password' : 'Set a password'} />
        </Form.Item>
        {isEdit && (
          <Text type="secondary" style={{ fontSize: 12, display: 'block', marginTop: -16, marginBottom: 8 }}>
            Leave password blank to keep the existing one.
          </Text>
        )}
      </Form>
    </Modal>
  );
}
