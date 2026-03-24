import { useState, useMemo } from 'react';
import {
  Table, Button, Tag, Space, Typography, Row, Col, Card,
  Modal, Form, Input, InputNumber, Select, Popconfirm, message, Tabs,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, ReloadOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { useStorageZones, type StorageZone } from '@/hooks/useStorageZones';
import { useCreateStorageZone, useUpdateStorageZone, useDeleteStorageZone } from '@/hooks/useStorageZoneMutations';
import { useRacks, type Rack } from '@/hooks/useRacks';
import { useCreateRack, useUpdateRack, useDeleteRack } from '@/hooks/useRackMutations';

const { Title } = Typography;

const ZONE_TYPES = ['Ambient', 'Cold Storage', 'Refrigerated', 'Controlled', 'Hazardous', 'Other'];

// ─── Storage Zone Form ───────────────────────────────────────────────────────

type ZoneFormValues = {
  name: string;
  zoneType: string;
  description?: string;
  temperatureRangeMin?: number;
  temperatureRangeMax?: number;
};

function StorageZoneFormModal({
  open, zone, onClose,
}: {
  open: boolean;
  zone: StorageZone | null;
  onClose: () => void;
}) {
  const [form] = Form.useForm<ZoneFormValues>();
  const create = useCreateStorageZone();
  const update = useUpdateStorageZone();

  const handleOk = async () => {
    const values = await form.validateFields();
    try {
      if (zone) {
        await update.mutateAsync({ id: zone.id, ...values, updatedBy: 'system' });
        message.success('Zone updated.');
      } else {
        await create.mutateAsync({ ...values, updatedBy: 'system' });
        message.success('Zone created.');
      }
      form.resetFields();
      onClose();
    } catch {
      message.error('Failed to save zone.');
    }
  };

  return (
    <Modal
      title={zone ? 'Edit Storage Zone' : 'New Storage Zone'}
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); onClose(); }}
      confirmLoading={create.isPending || update.isPending}
      destroyOnClose
    >
      <Form form={form} layout="vertical" style={{ marginTop: 16 }}
        initialValues={zone ?? {}}>
        <Form.Item name="name" label="Zone Name" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="zoneType" label="Zone Type" rules={[{ required: true }]}>
          <Select options={ZONE_TYPES.map(t => ({ value: t, label: t }))} />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={2} />
        </Form.Item>
        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="temperatureRangeMin" label="Temp Min (°C)">
              <InputNumber style={{ width: '100%' }} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="temperatureRangeMax" label="Temp Max (°C)">
              <InputNumber style={{ width: '100%' }} />
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Modal>
  );
}

// ─── Rack Form ───────────────────────────────────────────────────────────────

type RackFormValues = {
  storageZoneId: string;
  label: string;
  description?: string;
  capacity?: number;
};

function RackFormModal({
  open, rack, zones, onClose,
}: {
  open: boolean;
  rack: Rack | null;
  zones: StorageZone[];
  onClose: () => void;
}) {
  const [form] = Form.useForm<RackFormValues>();
  const create = useCreateRack();
  const update = useUpdateRack();

  const handleOk = async () => {
    const values = await form.validateFields();
    try {
      if (rack) {
        await update.mutateAsync({ id: rack.id, ...values, updatedBy: 'system' });
        message.success('Rack updated.');
      } else {
        await create.mutateAsync({ ...values, updatedBy: 'system' });
        message.success('Rack created.');
      }
      form.resetFields();
      onClose();
    } catch {
      message.error('Failed to save rack.');
    }
  };

  return (
    <Modal
      title={rack ? 'Edit Rack' : 'New Rack'}
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); onClose(); }}
      confirmLoading={create.isPending || update.isPending}
      destroyOnClose
    >
      <Form form={form} layout="vertical" style={{ marginTop: 16 }}
        initialValues={rack ?? {}}>
        <Form.Item name="storageZoneId" label="Storage Zone" rules={[{ required: true }]}>
          <Select
            showSearch
            options={zones.map(z => ({ value: z.id, label: z.name ?? z.id }))}
            filterOption={(i, o) => (o?.label ?? '').toLowerCase().includes(i.toLowerCase())}
          />
        </Form.Item>
        <Form.Item name="label" label="Rack Label" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={2} />
        </Form.Item>
        <Form.Item name="capacity" label="Capacity (units)">
          <InputNumber min={1} style={{ width: '100%' }} />
        </Form.Item>
      </Form>
    </Modal>
  );
}

// ─── Main Page ───────────────────────────────────────────────────────────────

export default function StorageZonesPage() {
  const [zoneModal, setZoneModal] = useState(false);
  const [editingZone, setEditingZone] = useState<StorageZone | null>(null);
  const [rackModal, setRackModal] = useState(false);
  const [editingRack, setEditingRack] = useState<Rack | null>(null);

  const zonesQuery = useStorageZones();
  const racksQuery = useRacks();
  const deleteZone = useDeleteStorageZone();
  const deleteRack = useDeleteRack();

  const zoneMap = useMemo(
    () => new Map((zonesQuery.data ?? []).map(z => [z.id, z.name ?? z.id])),
    [zonesQuery.data],
  );

  const handleDeleteZone = async (id: string) => {
    try { await deleteZone.mutateAsync(id); message.success('Zone deleted.'); }
    catch { message.error('Failed to delete zone.'); }
  };

  const handleDeleteRack = async (id: string) => {
    try { await deleteRack.mutateAsync(id); message.success('Rack deleted.'); }
    catch { message.error('Failed to delete rack.'); }
  };

  const zoneColumns: ColumnsType<StorageZone> = [
    { title: 'Name', dataIndex: 'name', render: (v: string | null) => v ?? '—' },
    {
      title: 'Type', dataIndex: 'zoneType', width: 130,
      render: (v: string | null) => v ? <Tag>{v}</Tag> : '—',
    },
    {
      title: 'Temp Range', key: 'temp', width: 130,
      render: (_: unknown, r: StorageZone) =>
        r.temperatureRangeMin != null || r.temperatureRangeMax != null
          ? `${r.temperatureRangeMin ?? '?'}°C – ${r.temperatureRangeMax ?? '?'}°C`
          : '—',
    },
    { title: 'Description', dataIndex: 'description', ellipsis: true, render: (v: string | null) => v ?? '—' },
    {
      title: 'Racks', key: 'rackCount', width: 80, align: 'right',
      render: (_: unknown, r: StorageZone) =>
        (racksQuery.data ?? []).filter(rack => rack.storageZoneId === r.id).length,
    },
    {
      title: 'Actions', key: 'actions', width: 80,
      render: (_: unknown, record: StorageZone) => (
        <Space size={4}>
          <Button type="text" size="small" icon={<EditOutlined />}
            onClick={() => { setEditingZone(record); setZoneModal(true); }} />
          <Popconfirm title="Delete zone?" onConfirm={() => handleDeleteZone(record.id)} okText="Delete" okType="danger">
            <Button type="text" size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  const rackColumns: ColumnsType<Rack> = [
    { title: 'Label', dataIndex: 'label', render: (v: string | null) => v ?? '—' },
    {
      title: 'Zone', dataIndex: 'storageZoneId', width: 160,
      render: (id: string) => zoneMap.get(id) ?? id.slice(0, 8),
    },
    { title: 'Capacity', dataIndex: 'capacity', align: 'right', width: 90, render: (v: number | null) => v ?? '—' },
    { title: 'Description', dataIndex: 'description', ellipsis: true, render: (v: string | null) => v ?? '—' },
    {
      title: 'Actions', key: 'actions', width: 80,
      render: (_: unknown, record: Rack) => (
        <Space size={4}>
          <Button type="text" size="small" icon={<EditOutlined />}
            onClick={() => { setEditingRack(record); setRackModal(true); }} />
          <Popconfirm title="Delete rack?" onConfirm={() => handleDeleteRack(record.id)} okText="Delete" okType="danger">
            <Button type="text" size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col><Title level={4} style={{ margin: 0 }}>Storage Zones & Racks</Title></Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        {[
          { label: 'Total Zones', value: (zonesQuery.data ?? []).length },
          { label: 'Total Racks', value: (racksQuery.data ?? []).length },
          { label: 'Total Capacity', value: (racksQuery.data ?? []).reduce((s, r) => s + (r.capacity ?? 0), 0) },
        ].map(s => (
          <Col xs={12} sm={6} key={s.label}>
            <Card size="small">
              <div style={{ fontSize: 12, color: '#888' }}>{s.label}</div>
              <div style={{ fontSize: 22, fontWeight: 700 }}>{s.value}</div>
            </Card>
          </Col>
        ))}
      </Row>

      <Tabs
        items={[
          {
            key: 'zones',
            label: 'Storage Zones',
            children: (
              <>
                <Row justify="end" style={{ marginBottom: 8 }}>
                  <Space>
                    <Button icon={<ReloadOutlined />} onClick={() => zonesQuery.refetch()} />
                    <Button type="primary" icon={<PlusOutlined />}
                      onClick={() => { setEditingZone(null); setZoneModal(true); }}>
                      New Zone
                    </Button>
                  </Space>
                </Row>
                <Table<StorageZone>
                  rowKey="id"
                  columns={zoneColumns}
                  dataSource={zonesQuery.data ?? []}
                  loading={zonesQuery.isFetching}
                  size="small"
                  pagination={{ pageSize: 20, showTotal: (t) => `${t} zones` }}
                />
              </>
            ),
          },
          {
            key: 'racks',
            label: 'Racks',
            children: (
              <>
                <Row justify="end" style={{ marginBottom: 8 }}>
                  <Space>
                    <Button icon={<ReloadOutlined />} onClick={() => racksQuery.refetch()} />
                    <Button type="primary" icon={<PlusOutlined />}
                      onClick={() => { setEditingRack(null); setRackModal(true); }}>
                      New Rack
                    </Button>
                  </Space>
                </Row>
                <Table<Rack>
                  rowKey="id"
                  columns={rackColumns}
                  dataSource={racksQuery.data ?? []}
                  loading={racksQuery.isFetching}
                  size="small"
                  pagination={{ pageSize: 20, showTotal: (t) => `${t} racks` }}
                />
              </>
            ),
          },
        ]}
      />

      <StorageZoneFormModal
        open={zoneModal}
        zone={editingZone}
        onClose={() => { setZoneModal(false); setEditingZone(null); }}
      />
      <RackFormModal
        open={rackModal}
        rack={editingRack}
        zones={zonesQuery.data ?? []}
        onClose={() => { setRackModal(false); setEditingRack(null); }}
      />
    </div>
  );
}
