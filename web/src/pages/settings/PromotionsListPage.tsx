import { useState } from 'react';
import { Button, Table, Space, Typography, Modal, Form, Input, InputNumber, DatePicker, message, Tag } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import dayjs from 'dayjs';
import axiosClient from '@/api/axiosClient';

const { Title } = Typography;

type Promotion = {
  id: string;
  name: string | null;
  description: string | null;
  discountPercentage: number;
  maxDiscountAmount: number | null;
  validFrom: string;
  validTo: string;
  applicableDrugId: string | null;
  applicableCategoryId: string | null;
};

function usePromotions() {
  return useQuery({
    queryKey: ['promotions'],
    queryFn: () => axiosClient.get<Promotion[]>('/promotions').then((r) => r.data),
  });
}

function useCreatePromotion() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: Partial<Promotion>) =>
      axiosClient.post<Promotion>('/promotions', data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['promotions'] }); },
  });
}

function useUpdatePromotion() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, ...data }: Partial<Promotion> & { id: string }) =>
      axiosClient.put<Promotion>(`/promotions/${id}`, data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['promotions'] }); },
  });
}

function useDeletePromotion() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => axiosClient.delete(`/promotions/${id}`),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['promotions'] }); },
  });
}

function isActive(p: Promotion) {
  const now = dayjs();
  return now.isAfter(dayjs(p.validFrom)) && now.isBefore(dayjs(p.validTo));
}

export default function PromotionsListPage() {
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Promotion | null>(null);
  const [form] = Form.useForm();
  const { data, isLoading } = usePromotions();
  const createMutation = useCreatePromotion();
  const updateMutation = useUpdatePromotion();
  const deleteMutation = useDeletePromotion();

  const openCreate = () => { setEditing(null); form.resetFields(); setOpen(true); };
  const openEdit = (p: Promotion) => {
    setEditing(p);
    form.setFieldsValue({
      ...p,
      validFrom: dayjs(p.validFrom),
      validTo: dayjs(p.validTo),
    });
    setOpen(true);
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const payload = {
      ...values,
      validFrom: (values.validFrom as dayjs.Dayjs)?.toISOString(),
      validTo: (values.validTo as dayjs.Dayjs)?.toISOString(),
    };
    try {
      if (editing) {
        await updateMutation.mutateAsync({ id: editing.id, ...payload });
        message.success('Promotion updated');
      } else {
        await createMutation.mutateAsync(payload);
        message.success('Promotion created');
      }
      setOpen(false);
    } catch {
      message.error('Failed to save promotion');
    }
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Delete promotion?',
      onOk: () => deleteMutation.mutateAsync(id).then(() => message.success('Deleted')),
    });
  };

  const columns: ColumnsType<Promotion> = [
    { title: 'Name', dataIndex: 'name', key: 'name' },
    { title: 'Discount %', dataIndex: 'discountPercentage', key: 'discountPercentage', render: (v: number) => `${v}%` },
    { title: 'Max Discount (₹)', dataIndex: 'maxDiscountAmount', key: 'maxDiscountAmount', render: (v: number | null) => v ?? '—' },
    { title: 'Valid From', dataIndex: 'validFrom', key: 'validFrom', render: (v: string) => dayjs(v).format('DD MMM YYYY') },
    { title: 'Valid To', dataIndex: 'validTo', key: 'validTo', render: (v: string) => dayjs(v).format('DD MMM YYYY') },
    {
      title: 'Status', key: 'status',
      render: (_: unknown, row: Promotion) => isActive(row)
        ? <Tag color="green">Active</Tag>
        : <Tag color="default">Inactive</Tag>,
    },
    {
      title: 'Actions', key: 'actions',
      render: (_: unknown, row: Promotion) => (
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
        <Title level={3} style={{ margin: 0 }}>Promotions</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openCreate}>Add Promotion</Button>
      </Space>
      <Table rowKey="id" dataSource={data ?? []} columns={columns} loading={isLoading} size="small" />
      <Modal
        title={editing ? 'Edit Promotion' : 'Add Promotion'}
        open={open}
        onOk={handleSubmit}
        onCancel={() => setOpen(false)}
        confirmLoading={createMutation.isPending || updateMutation.isPending}
        destroyOnClose
      >
        <Form form={form} layout="vertical">
          <Form.Item name="name" label="Name" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="description" label="Description">
            <Input.TextArea rows={2} />
          </Form.Item>
          <Form.Item name="discountPercentage" label="Discount %" rules={[{ required: true }]}>
            <InputNumber min={0.01} max={100} precision={2} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="maxDiscountAmount" label="Max Discount Amount (₹)">
            <InputNumber min={0} precision={2} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="validFrom" label="Valid From" rules={[{ required: true }]}>
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="validTo" label="Valid To" rules={[{ required: true }]}>
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
