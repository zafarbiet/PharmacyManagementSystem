import { useState } from 'react';
import { Button, Table, Space, Typography, Modal, Form, Select, Input, InputNumber, DatePicker, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import dayjs from 'dayjs';
import axiosClient from '@/api/axiosClient';

const { Title } = Typography;

type DrugPricing = {
  id: string;
  drugId: string;
  costPrice: number;
  sellingPrice: number;
  discount: number;
  gstRate: number;
  effectiveFrom: string;
  mrp: number;
  hsnCode: string | null;
};

type Drug = { id: string; name: string | null };

function useDrugPricings() {
  return useQuery({
    queryKey: ['drug-pricings'],
    queryFn: () => axiosClient.get<DrugPricing[]>('/drug-pricings').then((r) => r.data),
  });
}

function useDrugs() {
  return useQuery({
    queryKey: ['drugs'],
    queryFn: () => axiosClient.get<Drug[]>('/drugs').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

function useCreateDrugPricing() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: Partial<DrugPricing>) =>
      axiosClient.post<DrugPricing>('/drug-pricings', data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['drug-pricings'] }); },
  });
}

function useUpdateDrugPricing() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, ...data }: Partial<DrugPricing> & { id: string }) =>
      axiosClient.put<DrugPricing>(`/drug-pricings/${id}`, data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['drug-pricings'] }); },
  });
}

function useDeleteDrugPricing() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => axiosClient.delete(`/drug-pricings/${id}`),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['drug-pricings'] }); },
  });
}

export default function DrugPricingPage() {
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<DrugPricing | null>(null);
  const [form] = Form.useForm();

  const { data, isLoading } = useDrugPricings();
  const { data: drugs } = useDrugs();
  const createMutation = useCreateDrugPricing();
  const updateMutation = useUpdateDrugPricing();
  const deleteMutation = useDeleteDrugPricing();

  const drugMap = Object.fromEntries(drugs?.map((d) => [d.id, d.name ?? d.id]) ?? []);

  const openCreate = () => { setEditing(null); form.resetFields(); setOpen(true); };
  const openEdit = (p: DrugPricing) => {
    setEditing(p);
    form.setFieldsValue({ ...p, effectiveFrom: dayjs(p.effectiveFrom) });
    setOpen(true);
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const payload = {
      ...values,
      effectiveFrom: (values.effectiveFrom as dayjs.Dayjs).toISOString(),
    };
    try {
      if (editing) {
        await updateMutation.mutateAsync({ id: editing.id, ...payload });
        message.success('Pricing updated');
      } else {
        await createMutation.mutateAsync(payload);
        message.success('Pricing created');
      }
      setOpen(false);
    } catch {
      message.error('Failed to save pricing');
    }
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Delete pricing record?',
      onOk: () => deleteMutation.mutateAsync(id).then(() => message.success('Deleted')),
    });
  };

  const columns: ColumnsType<DrugPricing> = [
    { title: 'Drug', dataIndex: 'drugId', key: 'drugId', render: (v: string) => drugMap[v] ?? v },
    { title: 'Cost (₹)', dataIndex: 'costPrice', key: 'costPrice', render: (v: number) => v.toFixed(2) },
    { title: 'MRP (₹)', dataIndex: 'mrp', key: 'mrp', render: (v: number) => v.toFixed(2) },
    { title: 'Selling (₹)', dataIndex: 'sellingPrice', key: 'sellingPrice', render: (v: number) => v.toFixed(2) },
    { title: 'Discount %', dataIndex: 'discount', key: 'discount', render: (v: number) => `${v}%` },
    { title: 'GST %', dataIndex: 'gstRate', key: 'gstRate', render: (v: number) => `${v}%` },
    { title: 'HSN Code', dataIndex: 'hsnCode', key: 'hsnCode', render: (v: string | null) => v ?? '—' },
    { title: 'Effective From', dataIndex: 'effectiveFrom', key: 'effectiveFrom', render: (v: string) => dayjs(v).format('DD MMM YYYY') },
    {
      title: 'Actions', key: 'actions',
      render: (_: unknown, row: DrugPricing) => (
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
        <Title level={3} style={{ margin: 0 }}>Drug Pricing</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openCreate}>Add Pricing</Button>
      </Space>
      <Table rowKey="id" dataSource={data ?? []} columns={columns} loading={isLoading} size="small" />
      <Modal
        title={editing ? 'Edit Pricing' : 'Add Pricing'}
        open={open}
        onOk={handleSubmit}
        onCancel={() => setOpen(false)}
        confirmLoading={createMutation.isPending || updateMutation.isPending}
        destroyOnClose
      >
        <Form form={form} layout="vertical">
          <Form.Item name="drugId" label="Drug" rules={[{ required: true }]}>
            <Select
              options={drugs?.map((d) => ({ label: d.name ?? d.id, value: d.id }))}
              showSearch
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item name="costPrice" label="Cost Price (₹)" rules={[{ required: true }]}>
            <InputNumber min={0} precision={2} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="mrp" label="MRP (₹)" rules={[{ required: true }]}>
            <InputNumber min={0} precision={2} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="sellingPrice" label="Selling Price (₹)" rules={[{ required: true }]}>
            <InputNumber min={0} precision={2} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="discount" label="Discount %" rules={[{ required: true }]}>
            <InputNumber min={0} max={100} precision={2} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="gstRate" label="GST Rate %" rules={[{ required: true }]}>
            <InputNumber min={0} max={100} precision={2} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="effectiveFrom" label="Effective From" rules={[{ required: true }]}>
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="hsnCode" label="HSN Code">
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
