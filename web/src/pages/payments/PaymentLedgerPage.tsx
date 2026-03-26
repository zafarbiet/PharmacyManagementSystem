import { useState } from 'react';
import { Button, Table, Space, Typography, Modal, Form, Select, Input, InputNumber, DatePicker, message, Tag } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import dayjs from 'dayjs';
import axiosClient from '@/api/axiosClient';

const { Title } = Typography;

type PaymentLedger = {
  id: string;
  vendorId: string;
  purchaseOrderId: string | null;
  invoicedAmount: number;
  paidAmount: number;
  dueDate: string;
  status: string | null;
  notes: string | null;
};

type Vendor = { id: string; name: string | null };
type PurchaseOrder = { id: string; orderNumber: string | null };

const STATUS_OPTIONS = [
  { label: 'Pending', value: 'pending' },
  { label: 'Partial', value: 'partial' },
  { label: 'Paid', value: 'paid' },
  { label: 'Overdue', value: 'overdue' },
];

const STATUS_COLORS: Record<string, string> = {
  pending: 'orange',
  partial: 'blue',
  paid: 'green',
  overdue: 'red',
};

function usePaymentLedgers() {
  return useQuery({
    queryKey: ['payment-ledgers'],
    queryFn: () => axiosClient.get<PaymentLedger[]>('/payment-ledgers').then((r) => r.data),
  });
}

function useVendors() {
  return useQuery({
    queryKey: ['vendors'],
    queryFn: () => axiosClient.get<Vendor[]>('/vendors').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

function usePurchaseOrders() {
  return useQuery({
    queryKey: ['purchase-orders'],
    queryFn: () => axiosClient.get<PurchaseOrder[]>('/purchase-orders').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

function useCreatePaymentLedger() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: Partial<PaymentLedger>) =>
      axiosClient.post<PaymentLedger>('/payment-ledgers', data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['payment-ledgers'] }); },
  });
}

function useUpdatePaymentLedger() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, ...data }: Partial<PaymentLedger> & { id: string }) =>
      axiosClient.put<PaymentLedger>(`/payment-ledgers/${id}`, data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['payment-ledgers'] }); },
  });
}

function useDeletePaymentLedger() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => axiosClient.delete(`/payment-ledgers/${id}`),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['payment-ledgers'] }); },
  });
}

export default function PaymentLedgerPage() {
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<PaymentLedger | null>(null);
  const [form] = Form.useForm();

  const { data, isLoading } = usePaymentLedgers();
  const { data: vendors } = useVendors();
  const { data: purchaseOrders } = usePurchaseOrders();
  const createMutation = useCreatePaymentLedger();
  const updateMutation = useUpdatePaymentLedger();
  const deleteMutation = useDeletePaymentLedger();

  const vendorMap = Object.fromEntries(vendors?.map((v) => [v.id, v.name ?? v.id]) ?? []);
  const poMap = Object.fromEntries(purchaseOrders?.map((p) => [p.id, p.orderNumber ?? p.id]) ?? []);

  const openCreate = () => { setEditing(null); form.resetFields(); setOpen(true); };
  const openEdit = (r: PaymentLedger) => {
    setEditing(r);
    form.setFieldsValue({ ...r, dueDate: dayjs(r.dueDate) });
    setOpen(true);
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const payload = {
      ...values,
      dueDate: (values.dueDate as dayjs.Dayjs).toISOString(),
    };
    try {
      if (editing) {
        await updateMutation.mutateAsync({ id: editing.id, ...payload });
        message.success('Payment record updated');
      } else {
        await createMutation.mutateAsync(payload);
        message.success('Payment record created');
      }
      setOpen(false);
    } catch {
      message.error('Failed to save payment record');
    }
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Delete payment record?',
      onOk: () => deleteMutation.mutateAsync(id).then(() => message.success('Deleted')),
    });
  };

  const columns: ColumnsType<PaymentLedger> = [
    { title: 'Vendor', dataIndex: 'vendorId', key: 'vendorId', render: (v: string) => vendorMap[v] ?? v },
    { title: 'PO', dataIndex: 'purchaseOrderId', key: 'purchaseOrderId', render: (v: string | null) => v ? (poMap[v] ?? v) : '—' },
    { title: 'Invoiced (₹)', dataIndex: 'invoicedAmount', key: 'invoicedAmount', render: (v: number) => v.toFixed(2) },
    { title: 'Paid (₹)', dataIndex: 'paidAmount', key: 'paidAmount', render: (v: number) => v.toFixed(2) },
    {
      title: 'Balance (₹)', key: 'balance',
      render: (_: unknown, row: PaymentLedger) => (row.invoicedAmount - row.paidAmount).toFixed(2),
    },
    { title: 'Due Date', dataIndex: 'dueDate', key: 'dueDate', render: (v: string) => dayjs(v).format('DD MMM YYYY') },
    {
      title: 'Status', dataIndex: 'status', key: 'status',
      render: (v: string | null) => <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    { title: 'Notes', dataIndex: 'notes', key: 'notes', render: (v: string | null) => v ?? '—' },
    {
      title: 'Actions', key: 'actions',
      render: (_: unknown, row: PaymentLedger) => (
        <Space>
          <Button icon={<EditOutlined />} size="small" onClick={() => openEdit(row)} />
          <Button icon={<DeleteOutlined />} size="small" danger onClick={() => handleDelete(row.id)} />
        </Space>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Space style={{ marginBottom: 16, justifyContent: 'space-between', width: '100%' }}>
        <Title level={3} style={{ margin: 0 }}>Payment Ledger</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openCreate}>Add Payment</Button>
      </Space>
      <Table rowKey="id" dataSource={data ?? []} columns={columns} loading={isLoading} size="small" />
      <Modal
        title={editing ? 'Edit Payment' : 'Add Payment'}
        open={open}
        onOk={handleSubmit}
        onCancel={() => setOpen(false)}
        confirmLoading={createMutation.isPending || updateMutation.isPending}
        destroyOnClose
      >
        <Form form={form} layout="vertical">
          <Form.Item name="vendorId" label="Vendor" rules={[{ required: true }]}>
            <Select
              options={vendors?.map((v) => ({ label: v.name ?? v.id, value: v.id }))}
              showSearch
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item name="purchaseOrderId" label="Purchase Order">
            <Select
              options={purchaseOrders?.map((p) => ({ label: p.orderNumber ?? p.id, value: p.id }))}
              showSearch
              allowClear
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item name="invoicedAmount" label="Invoiced Amount (₹)" rules={[{ required: true }]}>
            <InputNumber min={0} precision={2} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="paidAmount" label="Paid Amount (₹)" rules={[{ required: true }]}>
            <InputNumber min={0} precision={2} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="dueDate" label="Due Date" rules={[{ required: true }]}>
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="status" label="Status">
            <Select options={STATUS_OPTIONS} />
          </Form.Item>
          <Form.Item name="notes" label="Notes">
            <Input.TextArea rows={3} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
