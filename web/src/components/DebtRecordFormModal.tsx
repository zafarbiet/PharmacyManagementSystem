import { useEffect } from 'react';
import { Modal, Form, Input, InputNumber, DatePicker, Select } from 'antd';
import dayjs from 'dayjs';
import type { DebtRecord } from '@/api/localTypes';
import type { Patient } from '@/api/localTypes';

const { TextArea } = Input;

interface Props {
  open: boolean;
  record: DebtRecord | null;
  patients: Patient[];
  onClose: () => void;
  onSubmit: (values: Partial<DebtRecord>) => Promise<void>;
  loading: boolean;
}

export default function DebtRecordFormModal({
  open, record, patients, onClose, onSubmit, loading,
}: Props) {
  const [form] = Form.useForm();

  useEffect(() => {
    if (open && record) {
      form.setFieldsValue({
        ...record,
        dueDate: record.dueDate ? dayjs(record.dueDate) : null,
      });
    } else if (open) {
      form.resetFields();
      form.setFieldsValue({ status: 'Open' });
    }
  }, [open, record, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    await onSubmit({
      ...values,
      dueDate: values.dueDate?.toISOString() ?? null,
    });
  };

  return (
    <Modal
      title={record ? 'Edit Debt Record' : 'New Debt Record'}
      open={open}
      onOk={handleOk}
      onCancel={onClose}
      okText="Save"
      confirmLoading={loading}
      destroyOnClose
    >
      <Form form={form} layout="vertical" style={{ marginTop: 12 }}>
        <Form.Item name="patientId" label="Patient" rules={[{ required: true }]}>
          <Select
            showSearch
            placeholder="Select patient"
            filterOption={(input, option) =>
              (option?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
            }
            options={patients.map((p) => ({ value: p.id, label: p.name ?? p.id }))}
          />
        </Form.Item>
        <Form.Item name="invoiceId" label="Invoice ID (GUID)" rules={[{ required: true }]}>
          <Input placeholder="Invoice GUID" />
        </Form.Item>
        <Form.Item name="originalAmount" label="Original Amount (₹)" rules={[{ required: true }]}>
          <InputNumber style={{ width: '100%' }} min={0} precision={2} prefix="₹" />
        </Form.Item>
        <Form.Item name="remainingAmount" label="Remaining Amount (₹)" rules={[{ required: true }]}>
          <InputNumber style={{ width: '100%' }} min={0} precision={2} prefix="₹" />
        </Form.Item>
        <Form.Item name="dueDate" label="Due Date">
          <DatePicker style={{ width: '100%' }} format="DD/MM/YYYY" />
        </Form.Item>
        <Form.Item name="status" label="Status">
          <Select>
            <Select.Option value="Open">Open</Select.Option>
            <Select.Option value="Partial">Partial</Select.Option>
            <Select.Option value="Paid">Paid</Select.Option>
            <Select.Option value="Written Off">Written Off</Select.Option>
          </Select>
        </Form.Item>
        <Form.Item name="notes" label="Notes">
          <TextArea rows={2} />
        </Form.Item>
      </Form>
    </Modal>
  );
}
