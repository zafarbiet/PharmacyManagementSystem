import { useEffect } from 'react';
import { Modal, Form, Input, Checkbox } from 'antd';
import type { Role } from '@/api/localTypes';

interface Props {
  open: boolean;
  role: Role | null;
  onClose: () => void;
  onSubmit: (values: Partial<Role>) => Promise<void>;
  loading: boolean;
}

const PERMISSION_BITS: { label: string; value: number }[] = [
  { label: 'Invoice: Read',       value: 1 << 0 },
  { label: 'Invoice: Create',     value: 1 << 1 },
  { label: 'Invoice: Void',       value: 1 << 2 },
  { label: 'PO: Read',            value: 1 << 3 },
  { label: 'PO: Create',          value: 1 << 4 },
  { label: 'PO: Approve',         value: 1 << 5 },
  { label: 'Inventory: Read',     value: 1 << 6 },
  { label: 'Inventory: Adjust',   value: 1 << 7 },
  { label: 'Drug: Read',          value: 1 << 8 },
  { label: 'Drug: Manage',        value: 1 << 9 },
  { label: 'Patient: Read',       value: 1 << 10 },
  { label: 'Patient: Manage',     value: 1 << 11 },
  { label: 'Reports: View',       value: 1 << 12 },
  { label: 'Reports: Export',     value: 1 << 13 },
  { label: 'Vendor: Read',        value: 1 << 14 },
  { label: 'Vendor: Manage',      value: 1 << 15 },
  { label: 'Users: Manage',       value: 1 << 16 },
  { label: 'Roles: Manage',       value: 1 << 17 },
  { label: 'Settings: Manage',    value: 1 << 18 },
  { label: 'Quotations: Manage',  value: 1 << 19 },
  { label: 'Debt: Manage',        value: 1 << 20 },
  { label: 'Audit Log: View',     value: 1 << 21 },
];

function bitmaskToArray(mask: number): number[] {
  return PERMISSION_BITS.filter((p) => (mask & p.value) !== 0).map((p) => p.value);
}

function arrayToBitmask(bits: number[]): number {
  return bits.reduce((acc, bit) => acc | bit, 0);
}

export default function RoleFormModal({ open, role, onClose, onSubmit, loading }: Props) {
  const [form] = Form.useForm();

  useEffect(() => {
    if (open && role) {
      form.setFieldsValue({
        ...role,
        permissionBits: bitmaskToArray(role.permissions ?? 0),
      });
    } else if (open) {
      form.resetFields();
    }
  }, [open, role, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    const { permissionBits, ...rest } = values;
    await onSubmit({ ...rest, permissions: arrayToBitmask(permissionBits ?? []) });
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
      width={600}
    >
      <Form form={form} layout="vertical" style={{ marginTop: 12 }}>
        <Form.Item name="name" label="Role Name" rules={[{ required: true, message: 'Name is required' }]}>
          <Input placeholder="e.g. Pharmacist, Manager" />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={2} placeholder="What this role can do" />
        </Form.Item>
        <Form.Item name="permissionBits" label="Permissions">
          <Checkbox.Group
            options={PERMISSION_BITS}
            style={{
              display: 'grid',
              gridTemplateColumns: '1fr 1fr',
              gap: '4px 16px',
            }}
          />
        </Form.Item>
      </Form>
    </Modal>
  );
}
