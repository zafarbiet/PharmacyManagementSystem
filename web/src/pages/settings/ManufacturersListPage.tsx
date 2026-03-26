import { useState } from 'react';
import { Button, Table, Space, Typography, Modal, Form, Input, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';

const { Title } = Typography;

type Manufacturer = {
  id: string;
  name: string | null;
  contactEmail: string | null;
  contactPhone: string | null;
  country: string | null;
  address: string | null;
  gstin: string | null;
  drugLicenseNumber: string | null;
};

function useManufacturers() {
  return useQuery({
    queryKey: ['manufacturers'],
    queryFn: () => axiosClient.get<Manufacturer[]>('/manufacturers').then((r) => r.data),
  });
}

function useCreateManufacturer() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: Partial<Manufacturer>) =>
      axiosClient.post<Manufacturer>('/manufacturers', data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['manufacturers'] }); },
  });
}

function useUpdateManufacturer() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, ...data }: Partial<Manufacturer> & { id: string }) =>
      axiosClient.put<Manufacturer>(`/manufacturers/${id}`, data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['manufacturers'] }); },
  });
}

function useDeleteManufacturer() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => axiosClient.delete(`/manufacturers/${id}`),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['manufacturers'] }); },
  });
}

export default function ManufacturersListPage() {
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Manufacturer | null>(null);
  const [form] = Form.useForm();
  const { data, isLoading } = useManufacturers();
  const createMutation = useCreateManufacturer();
  const updateMutation = useUpdateManufacturer();
  const deleteMutation = useDeleteManufacturer();

  const openCreate = () => { setEditing(null); form.resetFields(); setOpen(true); };
  const openEdit = (m: Manufacturer) => { setEditing(m); form.setFieldsValue(m); setOpen(true); };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    try {
      if (editing) {
        await updateMutation.mutateAsync({ id: editing.id, ...values });
        message.success('Manufacturer updated');
      } else {
        await createMutation.mutateAsync(values);
        message.success('Manufacturer created');
      }
      setOpen(false);
    } catch {
      message.error('Failed to save manufacturer');
    }
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Delete manufacturer?',
      onOk: () => deleteMutation.mutateAsync(id).then(() => message.success('Deleted')),
    });
  };

  const columns: ColumnsType<Manufacturer> = [
    { title: 'Name', dataIndex: 'name', key: 'name' },
    { title: 'Country', dataIndex: 'country', key: 'country' },
    { title: 'Contact Email', dataIndex: 'contactEmail', key: 'contactEmail' },
    { title: 'Contact Phone', dataIndex: 'contactPhone', key: 'contactPhone' },
    { title: 'GSTIN', dataIndex: 'gstin', key: 'gstin' },
    { title: 'Drug License No.', dataIndex: 'drugLicenseNumber', key: 'drugLicenseNumber' },
    {
      title: 'Actions', key: 'actions',
      render: (_, row) => (
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
        <Title level={3} style={{ margin: 0 }}>Manufacturers</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openCreate}>Add Manufacturer</Button>
      </Space>
      <Table rowKey="id" dataSource={data ?? []} columns={columns} loading={isLoading} size="small" />
      <Modal
        title={editing ? 'Edit Manufacturer' : 'Add Manufacturer'}
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
          <Form.Item name="country" label="Country">
            <Input />
          </Form.Item>
          <Form.Item name="address" label="Address">
            <Input.TextArea rows={2} />
          </Form.Item>
          <Form.Item name="contactEmail" label="Contact Email">
            <Input />
          </Form.Item>
          <Form.Item name="contactPhone" label="Contact Phone">
            <Input />
          </Form.Item>
          <Form.Item name="gstin" label="GSTIN">
            <Input />
          </Form.Item>
          <Form.Item name="drugLicenseNumber" label="Drug License No.">
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
