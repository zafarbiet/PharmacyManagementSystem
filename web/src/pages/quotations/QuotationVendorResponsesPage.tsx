import { useState } from 'react';
import { Button, Table, Space, Typography, Modal, Form, Select, Input, DatePicker, message, Tag } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import dayjs from 'dayjs';
import axiosClient from '@/api/axiosClient';

const { Title } = Typography;

type QuotationVendorResponse = {
  id: string;
  quotationRequestId: string;
  vendorId: string;
  status: string | null;
  respondedAt: string | null;
  notes: string | null;
};

type Vendor = { id: string; name: string | null };
type QuotationRequest = { id: string; status: string | null; requestDate: string };

const STATUS_OPTIONS = [
  { label: 'Pending', value: 'pending' },
  { label: 'Responded', value: 'responded' },
  { label: 'Accepted', value: 'accepted' },
  { label: 'Rejected', value: 'rejected' },
  { label: 'No Response', value: 'no_response' },
];

const STATUS_COLORS: Record<string, string> = {
  pending: 'orange',
  responded: 'blue',
  accepted: 'green',
  rejected: 'red',
  no_response: 'default',
};

function useVendorResponses() {
  return useQuery({
    queryKey: ['quotation-vendor-responses'],
    queryFn: () => axiosClient.get<QuotationVendorResponse[]>('/quotation-vendor-responses').then((r) => r.data),
  });
}

function useVendors() {
  return useQuery({
    queryKey: ['vendors'],
    queryFn: () => axiosClient.get<Vendor[]>('/vendors').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

function useQuotationRequests() {
  return useQuery({
    queryKey: ['quotation-requests'],
    queryFn: () => axiosClient.get<QuotationRequest[]>('/quotation-requests').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

function useCreateVendorResponse() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: Partial<QuotationVendorResponse>) =>
      axiosClient.post<QuotationVendorResponse>('/quotation-vendor-responses', data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['quotation-vendor-responses'] }); },
  });
}

function useUpdateVendorResponse() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, ...data }: Partial<QuotationVendorResponse> & { id: string }) =>
      axiosClient.put<QuotationVendorResponse>(`/quotation-vendor-responses/${id}`, data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['quotation-vendor-responses'] }); },
  });
}

function useDeleteVendorResponse() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => axiosClient.delete(`/quotation-vendor-responses/${id}`),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['quotation-vendor-responses'] }); },
  });
}

export default function QuotationVendorResponsesPage() {
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<QuotationVendorResponse | null>(null);
  const [form] = Form.useForm();

  const { data, isLoading } = useVendorResponses();
  const { data: vendors } = useVendors();
  const { data: quotationRequests } = useQuotationRequests();
  const createMutation = useCreateVendorResponse();
  const updateMutation = useUpdateVendorResponse();
  const deleteMutation = useDeleteVendorResponse();

  const vendorMap = Object.fromEntries(vendors?.map((v) => [v.id, v.name ?? v.id]) ?? []);
  const requestMap = Object.fromEntries(
    quotationRequests?.map((r) => [r.id, dayjs(r.requestDate).format('DD MMM YYYY')]) ?? [],
  );

  const openCreate = () => { setEditing(null); form.resetFields(); setOpen(true); };
  const openEdit = (r: QuotationVendorResponse) => {
    setEditing(r);
    form.setFieldsValue({ ...r, respondedAt: r.respondedAt ? dayjs(r.respondedAt) : null });
    setOpen(true);
  };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    const payload = {
      ...values,
      respondedAt: values.respondedAt ? (values.respondedAt as dayjs.Dayjs).toISOString() : null,
    };
    try {
      if (editing) {
        await updateMutation.mutateAsync({ id: editing.id, ...payload });
        message.success('Response updated');
      } else {
        await createMutation.mutateAsync(payload);
        message.success('Response created');
      }
      setOpen(false);
    } catch {
      message.error('Failed to save response');
    }
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Delete response?',
      onOk: () => deleteMutation.mutateAsync(id).then(() => message.success('Deleted')),
    });
  };

  const columns: ColumnsType<QuotationVendorResponse> = [
    {
      title: 'Quotation Request', dataIndex: 'quotationRequestId', key: 'quotationRequestId',
      render: (v: string) => requestMap[v] ?? v,
    },
    { title: 'Vendor', dataIndex: 'vendorId', key: 'vendorId', render: (v: string) => vendorMap[v] ?? v },
    {
      title: 'Status', dataIndex: 'status', key: 'status',
      render: (v: string | null) => <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    {
      title: 'Responded At', dataIndex: 'respondedAt', key: 'respondedAt',
      render: (v: string | null) => v ? dayjs(v).format('DD MMM YYYY HH:mm') : '—',
    },
    { title: 'Notes', dataIndex: 'notes', key: 'notes', render: (v: string | null) => v ?? '—' },
    {
      title: 'Actions', key: 'actions',
      render: (_: unknown, row: QuotationVendorResponse) => (
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
        <Title level={3} style={{ margin: 0 }}>Quotation Vendor Responses</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openCreate}>Add Response</Button>
      </Space>
      <Table rowKey="id" dataSource={data ?? []} columns={columns} loading={isLoading} size="small" />
      <Modal
        title={editing ? 'Edit Response' : 'Add Response'}
        open={open}
        onOk={handleSubmit}
        onCancel={() => setOpen(false)}
        confirmLoading={createMutation.isPending || updateMutation.isPending}
        destroyOnClose
      >
        <Form form={form} layout="vertical">
          <Form.Item name="quotationRequestId" label="Quotation Request" rules={[{ required: true }]}>
            <Select
              options={quotationRequests?.map((r) => ({
                label: `RFQ — ${dayjs(r.requestDate).format('DD MMM YYYY')}`,
                value: r.id,
              }))}
              showSearch
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item name="vendorId" label="Vendor" rules={[{ required: true }]}>
            <Select
              options={vendors?.map((v) => ({ label: v.name ?? v.id, value: v.id }))}
              showSearch
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item name="status" label="Status">
            <Select options={STATUS_OPTIONS} />
          </Form.Item>
          <Form.Item name="respondedAt" label="Responded At">
            <DatePicker showTime style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="notes" label="Notes">
            <Input.TextArea rows={3} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
