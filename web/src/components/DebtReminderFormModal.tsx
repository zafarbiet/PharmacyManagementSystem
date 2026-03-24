import { useEffect } from 'react';
import { Modal, Form, DatePicker, Select, Input } from 'antd';
import dayjs from 'dayjs';
import type { DebtReminder } from '@/api/localTypes';

const { TextArea } = Input;

interface Props {
  open: boolean;
  debtRecordId: string;
  onClose: () => void;
  onSubmit: (values: Partial<DebtReminder>) => Promise<void>;
  loading: boolean;
}

export default function DebtReminderFormModal({
  open, debtRecordId, onClose, onSubmit, loading,
}: Props) {
  const [form] = Form.useForm();

  useEffect(() => {
    if (open) {
      form.setFieldsValue({ sentAt: dayjs(), debtRecordId });
    } else {
      form.resetFields();
    }
  }, [open, debtRecordId, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    await onSubmit({
      ...values,
      debtRecordId,
      sentAt: values.sentAt?.toISOString(),
    });
  };

  return (
    <Modal
      title="Log Reminder"
      open={open}
      onOk={handleOk}
      onCancel={onClose}
      okText="Save"
      confirmLoading={loading}
      destroyOnClose
    >
      <Form form={form} layout="vertical" style={{ marginTop: 12 }}>
        <Form.Item name="sentAt" label="Sent At" rules={[{ required: true }]}>
          <DatePicker showTime style={{ width: '100%' }} format="DD/MM/YYYY HH:mm" />
        </Form.Item>
        <Form.Item name="channel" label="Channel" rules={[{ required: true }]}>
          <Select placeholder="Select channel">
            <Select.Option value="SMS">SMS</Select.Option>
            <Select.Option value="WhatsApp">WhatsApp</Select.Option>
            <Select.Option value="Phone">Phone Call</Select.Option>
            <Select.Option value="Email">Email</Select.Option>
            <Select.Option value="In-Person">In-Person</Select.Option>
          </Select>
        </Form.Item>
        <Form.Item name="sentBy" label="Sent By">
          <Input placeholder="Staff name" />
        </Form.Item>
        <Form.Item name="message" label="Message / Notes">
          <TextArea rows={3} />
        </Form.Item>
      </Form>
    </Modal>
  );
}
