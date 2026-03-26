import { useEffect } from 'react';
import { Modal, Form, Input, InputNumber, Checkbox, Divider } from 'antd';
import type { Patient } from '@/api/localTypes';

interface Props {
  open: boolean;
  patient: Patient | null;
  onClose: () => void;
  onSubmit: (values: Partial<Patient>) => Promise<void>;
  loading: boolean;
}

export default function PatientFormModal({ open, patient, onClose, onSubmit, loading }: Props) {
  const [form] = Form.useForm<Partial<Patient>>();

  useEffect(() => {
    if (open) {
      form.resetFields();
      if (patient) form.setFieldsValue(patient);
    }
  }, [open, patient, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    await onSubmit(values);
  };

  return (
    <Modal
      title={patient ? 'Edit Patient' : 'New Patient'}
      open={open}
      onOk={handleOk}
      onCancel={onClose}
      okText={patient ? 'Save' : 'Create'}
      confirmLoading={loading}
      destroyOnClose
      width={520}
    >
      <Form form={form} layout="vertical" style={{ marginTop: 16 }}>
        <Form.Item name="name" label="Full Name" rules={[{ required: true, message: 'Required' }]}>
          <Input placeholder="Patient name" />
        </Form.Item>
        <Form.Item name="contactNumber" label="Contact Number">
          <Input placeholder="+91 XXXXX XXXXX" />
        </Form.Item>
        <Form.Item name="email" label="Email">
          <Input placeholder="patient@email.com" />
        </Form.Item>
        <Form.Item name="address" label="Address">
          <Input.TextArea rows={2} placeholder="Address" />
        </Form.Item>
        <Form.Item name="age" label="Age">
          <InputNumber min={0} max={150} style={{ width: '100%' }} placeholder="Age" />
        </Form.Item>
        <Form.Item name="gstin" label="GSTIN">
          <Input placeholder="15-digit GSTIN (if applicable)" maxLength={15} />
        </Form.Item>
        <Form.Item name="isSubscriber" valuePropName="checked">
          <Checkbox>Subscription Patient</Checkbox>
        </Form.Item>
        <Divider style={{ margin: '8px 0' }} />
        <Form.Item
          name="creditLimit"
          label="Credit Limit (₹)"
          tooltip="Maximum invoice total allowed for on-account purchases. Set to 0 for no limit."
        >
          <InputNumber min={0} precision={2} style={{ width: '100%' }} placeholder="0 = unlimited" />
        </Form.Item>
      </Form>
    </Modal>
  );
}
