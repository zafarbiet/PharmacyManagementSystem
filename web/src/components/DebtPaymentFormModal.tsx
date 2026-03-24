import { useEffect } from 'react';
import { Modal, Form, InputNumber, DatePicker, Select, Input } from 'antd';
import dayjs from 'dayjs';
import type { DebtPayment } from '@/api/localTypes';

const { TextArea } = Input;

interface Props {
  open: boolean;
  debtRecordId: string;
  maxAmount?: number;
  onClose: () => void;
  onSubmit: (values: Partial<DebtPayment>) => Promise<void>;
  loading: boolean;
}

export default function DebtPaymentFormModal({
  open, debtRecordId, maxAmount, onClose, onSubmit, loading,
}: Props) {
  const [form] = Form.useForm();

  useEffect(() => {
    if (open) {
      form.setFieldsValue({ paymentDate: dayjs(), debtRecordId });
    } else {
      form.resetFields();
    }
  }, [open, debtRecordId, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    await onSubmit({
      ...values,
      debtRecordId,
      paymentDate: values.paymentDate?.toISOString(),
    });
  };

  return (
    <Modal
      title="Record Payment"
      open={open}
      onOk={handleOk}
      onCancel={onClose}
      okText="Save"
      confirmLoading={loading}
      destroyOnClose
    >
      <Form form={form} layout="vertical" style={{ marginTop: 12 }}>
        <Form.Item name="paymentDate" label="Payment Date" rules={[{ required: true }]}>
          <DatePicker style={{ width: '100%' }} format="DD/MM/YYYY" />
        </Form.Item>
        <Form.Item name="amountPaid" label="Amount Paid (₹)" rules={[{ required: true }]}>
          <InputNumber
            style={{ width: '100%' }}
            min={0.01}
            max={maxAmount}
            precision={2}
            prefix="₹"
          />
        </Form.Item>
        <Form.Item name="paymentMethod" label="Payment Method">
          <Select allowClear placeholder="Select method">
            <Select.Option value="Cash">Cash</Select.Option>
            <Select.Option value="UPI">UPI</Select.Option>
            <Select.Option value="Card">Card</Select.Option>
            <Select.Option value="Bank Transfer">Bank Transfer</Select.Option>
            <Select.Option value="Cheque">Cheque</Select.Option>
          </Select>
        </Form.Item>
        <Form.Item name="receivedBy" label="Received By">
          <Input placeholder="Staff name" />
        </Form.Item>
        <Form.Item name="notes" label="Notes">
          <TextArea rows={2} />
        </Form.Item>
      </Form>
    </Modal>
  );
}
