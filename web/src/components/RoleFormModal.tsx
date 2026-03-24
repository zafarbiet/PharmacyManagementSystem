import { useEffect } from 'react';
import { Modal, Form, Input } from 'antd';
import type { Role } from '@/api/localTypes';

interface Props {
  open: boolean;
  role: Role | null;
  onClose: () => void;
  onSubmit: (values: Partial<Role>) => Promise<void>;
  loading: boolean;
}

export default function RoleFormModal({ open, role, onClose, onSubmit, loading }: Props) {
  const [form] = Form.useForm();

  useEffect(() => {
    if (open && role) {
      form.setFieldsValue(role);
    } else if (open) {
      form.resetFields();
    }
  }, [open, role, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    await onSubmit(values);
  };

  return (
    <Modal
      title={role ? 'Edit Role' : 'New Role'}
      open={open}
      onOk={handleOk}
      onCancel={onClose}
      okText="Save"
      confirmLoading={loading}
      destroyOnClose
    >
      <Form form={form} layout="vertical" style={{ marginTop: 12 }}>
        <Form.Item name="name" label="Role Name" rules={[{ required: true, message: 'Name is required' }]}>
          <Input placeholder="e.g. Pharmacist, Manager" />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={2} placeholder="What this role can do" />
        </Form.Item>
      </Form>
    </Modal>
  );
}
