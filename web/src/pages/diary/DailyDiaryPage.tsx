import { useState } from 'react';
import { Button, Table, Space, Typography, Modal, Form, Select, Input, DatePicker, message, Tag } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import dayjs from 'dayjs';
import axiosClient from '@/api/axiosClient';

const { Title } = Typography;

type DailyDiaryEntry = {
  id: string;
  entryDate: string;
  category: string | null;
  title: string | null;
  body: string | null;
  drugId: string | null;
  vendorId: string | null;
  patientId: string | null;
  priority: string | null;
  createdBy: string | null;
};

type Drug = { id: string; name: string | null };
type Vendor = { id: string; name: string | null };
type Patient = { id: string; name: string | null };

const CATEGORY_OPTIONS = [
  { label: 'General', value: 'general' },
  { label: 'Drug', value: 'drug' },
  { label: 'Vendor', value: 'vendor' },
  { label: 'Patient', value: 'patient' },
  { label: 'Financial', value: 'financial' },
  { label: 'Regulatory', value: 'regulatory' },
];

const PRIORITY_OPTIONS = [
  { label: 'Low', value: 'low' },
  { label: 'Medium', value: 'medium' },
  { label: 'High', value: 'high' },
];

const PRIORITY_COLORS: Record<string, string> = { low: 'default', medium: 'blue', high: 'red' };

function useDiaryEntries() {
  return useQuery({
    queryKey: ['diary-entries'],
    queryFn: () => axiosClient.get<DailyDiaryEntry[]>('/daily-diary').then((r) => r.data),
  });
}

function useDrugs() {
  return useQuery({
    queryKey: ['drugs'],
    queryFn: () => axiosClient.get<Drug[]>('/drugs').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

function useVendors() {
  return useQuery({
    queryKey: ['vendors'],
    queryFn: () => axiosClient.get<Vendor[]>('/vendors').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

function usePatients() {
  return useQuery({
    queryKey: ['patients'],
    queryFn: () => axiosClient.get<Patient[]>('/patients').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

function useCreateDiaryEntry() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: Partial<DailyDiaryEntry>) =>
      axiosClient.post<DailyDiaryEntry>('/daily-diary', data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['diary-entries'] }); },
  });
}

function useUpdateDiaryEntry() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, ...data }: Partial<DailyDiaryEntry> & { id: string }) =>
      axiosClient.put<DailyDiaryEntry>(`/daily-diary/${id}`, data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['diary-entries'] }); },
  });
}

function useDeleteDiaryEntry() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => axiosClient.delete(`/daily-diary/${id}`),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['diary-entries'] }); },
  });
}

export default function DailyDiaryPage() {
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<DailyDiaryEntry | null>(null);
  const [form] = Form.useForm();

  const { data, isLoading } = useDiaryEntries();
  const { data: drugs } = useDrugs();
  const { data: vendors } = useVendors();
  const { data: patients } = usePatients();
  const createMutation = useCreateDiaryEntry();
  const updateMutation = useUpdateDiaryEntry();
  const deleteMutation = useDeleteDiaryEntry();

  const drugMap = Object.fromEntries(drugs?.map((d) => [d.id, d.name ?? d.id]) ?? []);
  const vendorMap = Object.fromEntries(vendors?.map((v) => [v.id, v.name ?? v.id]) ?? []);
  const patientMap = Object.fromEntries(patients?.map((p) => [p.id, p.name ?? p.id]) ?? []);

  const openCreate = () => { setEditing(null); form.resetFields(); setOpen(true); };
  const openEdit = (e: DailyDiaryEntry) => {
    setEditing(e);
    form.setFieldsValue({ ...e, entryDate: dayjs(e.entryDate) });
    setOpen(true);
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const payload = {
      ...values,
      entryDate: (values.entryDate as dayjs.Dayjs).toISOString(),
    };
    try {
      if (editing) {
        await updateMutation.mutateAsync({ id: editing.id, ...payload });
        message.success('Entry updated');
      } else {
        await createMutation.mutateAsync(payload);
        message.success('Entry created');
      }
      setOpen(false);
    } catch {
      message.error('Failed to save entry');
    }
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Delete diary entry?',
      onOk: () => deleteMutation.mutateAsync(id).then(() => message.success('Deleted')),
    });
  };

  const columns: ColumnsType<DailyDiaryEntry> = [
    { title: 'Date', dataIndex: 'entryDate', key: 'entryDate', render: (v: string) => dayjs(v).format('DD MMM YYYY') },
    { title: 'Category', dataIndex: 'category', key: 'category' },
    { title: 'Title', dataIndex: 'title', key: 'title' },
    {
      title: 'Priority', dataIndex: 'priority', key: 'priority',
      render: (v: string | null) => <Tag color={PRIORITY_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    { title: 'Drug', dataIndex: 'drugId', key: 'drugId', render: (v: string | null) => v ? (drugMap[v] ?? v) : '—' },
    { title: 'Vendor', dataIndex: 'vendorId', key: 'vendorId', render: (v: string | null) => v ? (vendorMap[v] ?? v) : '—' },
    { title: 'Patient', dataIndex: 'patientId', key: 'patientId', render: (v: string | null) => v ? (patientMap[v] ?? v) : '—' },
    { title: 'Created By', dataIndex: 'createdBy', key: 'createdBy', render: (v: string | null) => v ?? '—' },
    {
      title: 'Actions', key: 'actions',
      render: (_: unknown, row: DailyDiaryEntry) => (
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
        <Title level={3} style={{ margin: 0 }}>Daily Diary</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openCreate}>Add Entry</Button>
      </Space>
      <Table rowKey="id" dataSource={data ?? []} columns={columns} loading={isLoading} size="small" />
      <Modal
        title={editing ? 'Edit Entry' : 'Add Entry'}
        open={open}
        onOk={handleSubmit}
        onCancel={() => setOpen(false)}
        confirmLoading={createMutation.isPending || updateMutation.isPending}
        destroyOnClose
        width={600}
      >
        <Form form={form} layout="vertical">
          <Form.Item name="entryDate" label="Entry Date" rules={[{ required: true }]}>
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="category" label="Category">
            <Select options={CATEGORY_OPTIONS} />
          </Form.Item>
          <Form.Item name="title" label="Title" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="body" label="Notes">
            <Input.TextArea rows={4} />
          </Form.Item>
          <Form.Item name="priority" label="Priority">
            <Select options={PRIORITY_OPTIONS} />
          </Form.Item>
          <Form.Item name="drugId" label="Drug (optional)">
            <Select
              options={drugs?.map((d) => ({ label: d.name ?? d.id, value: d.id }))}
              showSearch allowClear
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item name="vendorId" label="Vendor (optional)">
            <Select
              options={vendors?.map((v) => ({ label: v.name ?? v.id, value: v.id }))}
              showSearch allowClear
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item name="patientId" label="Patient (optional)">
            <Select
              options={patients?.map((p) => ({ label: p.name ?? p.id, value: p.id }))}
              showSearch allowClear
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item name="createdBy" label="Created By">
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
