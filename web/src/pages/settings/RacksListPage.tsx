import { useState } from 'react';
import { Button, Table, Space, Typography, Modal, Form, Select, Input, InputNumber, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';

const { Title } = Typography;

type Rack = {
  id: string;
  storageZoneId: string;
  label: string | null;
  description: string | null;
  capacity: number | null;
};

type StorageZone = { id: string; name: string | null };

function useRacks() {
  return useQuery({
    queryKey: ['racks'],
    queryFn: () => axiosClient.get<Rack[]>('/racks').then((r) => r.data),
  });
}

function useStorageZones() {
  return useQuery({
    queryKey: ['storage-zones'],
    queryFn: () => axiosClient.get<StorageZone[]>('/storage-zones').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

function useCreateRack() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: Partial<Rack>) =>
      axiosClient.post<Rack>('/racks', data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['racks'] }); },
  });
}

function useUpdateRack() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, ...data }: Partial<Rack> & { id: string }) =>
      axiosClient.put<Rack>(`/racks/${id}`, data).then((r) => r.data),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['racks'] }); },
  });
}

function useDeleteRack() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => axiosClient.delete(`/racks/${id}`),
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['racks'] }); },
  });
}

export default function RacksListPage() {
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Rack | null>(null);
  const [form] = Form.useForm();

  const { data, isLoading } = useRacks();
  const { data: storageZones } = useStorageZones();
  const createMutation = useCreateRack();
  const updateMutation = useUpdateRack();
  const deleteMutation = useDeleteRack();

  const zoneMap = Object.fromEntries(storageZones?.map((z) => [z.id, z.name ?? z.id]) ?? []);

  const openCreate = () => { setEditing(null); form.resetFields(); setOpen(true); };
  const openEdit = (r: Rack) => { setEditing(r); form.setFieldsValue(r); setOpen(true); };

  const handleSubmit = async () => {
    const values = await form.validateFields();
    try {
      if (editing) {
        await updateMutation.mutateAsync({ id: editing.id, ...values });
        message.success('Rack updated');
      } else {
        await createMutation.mutateAsync(values);
        message.success('Rack created');
      }
      setOpen(false);
    } catch {
      message.error('Failed to save rack');
    }
  };

  const handleDelete = (id: string) => {
    Modal.confirm({
      title: 'Delete rack?',
      onOk: () => deleteMutation.mutateAsync(id).then(() => message.success('Deleted')),
    });
  };

  const columns: ColumnsType<Rack> = [
    { title: 'Label', dataIndex: 'label', key: 'label' },
    { title: 'Storage Zone', dataIndex: 'storageZoneId', key: 'storageZoneId', render: (v: string) => zoneMap[v] ?? v },
    { title: 'Capacity', dataIndex: 'capacity', key: 'capacity', render: (v: number | null) => v ?? '—' },
    { title: 'Description', dataIndex: 'description', key: 'description', render: (v: string | null) => v ?? '—' },
    {
      title: 'Actions', key: 'actions',
      render: (_: unknown, row: Rack) => (
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
        <Title level={3} style={{ margin: 0 }}>Racks</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openCreate}>Add Rack</Button>
      </Space>
      <Table rowKey="id" dataSource={data ?? []} columns={columns} loading={isLoading} size="small" />
      <Modal
        title={editing ? 'Edit Rack' : 'Add Rack'}
        open={open}
        onOk={handleSubmit}
        onCancel={() => setOpen(false)}
        confirmLoading={createMutation.isPending || updateMutation.isPending}
        destroyOnClose
      >
        <Form form={form} layout="vertical">
          <Form.Item name="label" label="Label" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="storageZoneId" label="Storage Zone" rules={[{ required: true }]}>
            <Select
              options={storageZones?.map((z) => ({ label: z.name ?? z.id, value: z.id }))}
              showSearch
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item name="capacity" label="Capacity">
            <InputNumber min={1} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="description" label="Description">
            <Input.TextArea rows={2} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
