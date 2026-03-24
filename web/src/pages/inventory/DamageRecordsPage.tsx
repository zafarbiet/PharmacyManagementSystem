import { useState, useMemo } from 'react';
import {
  Table, Button, Tag, Space, Typography, Row, Col, Card,
  Modal, Form, Select, InputNumber, DatePicker, Input, Popconfirm, message,
} from 'antd';
import { PlusOutlined, DeleteOutlined, ReloadOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { useDamageRecords, useCreateDamageRecord, useDeleteDamageRecord, type DamageRecord } from '@/hooks/useDamageRecords';
import { useDrugInventory } from '@/hooks/useDrugInventory';
import { useDrugs } from '@/hooks/useDrugs';

const { Title } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Detected: 'orange', Quarantined: 'gold', Disposed: 'red', Resolved: 'green',
};
const DAMAGE_TYPES = ['Physical', 'Chemical', 'Biological', 'Expired', 'Manufacturing', 'Other'];

type FormValues = {
  drugInventoryId: string;
  quantityDamaged: number;
  damageType: string;
  damagedAt: dayjs.Dayjs;
  discoveredBy: string;
  notes?: string;
};

export default function DamageRecordsPage() {
  const [statusFilter, setStatusFilter] = useState<string | undefined>();
  const [modalOpen, setModalOpen] = useState(false);
  const [form] = Form.useForm<FormValues>();

  const recordsQuery = useDamageRecords({ status: statusFilter });
  const inventoryQuery = useDrugInventory();
  const drugsQuery = useDrugs();
  const createRecord = useCreateDamageRecord();
  const deleteRecord = useDeleteDamageRecord();

  const drugMap = useMemo(() => new Map((drugsQuery.data ?? []).map((d) => [d.id ?? '', d.name ?? d.id ?? ''])), [drugsQuery.data]);
  const invMap = useMemo(() => new Map((inventoryQuery.data ?? []).map((i) => [i.id, i])), [inventoryQuery.data]);

  const invOptions = useMemo(() =>
    (inventoryQuery.data ?? []).map((i) => ({
      value: i.id,
      label: `${drugMap.get(i.drugId) ?? i.drugId} — Batch ${i.batchNumber ?? '?'} (${i.quantityInStock} units)`,
    })), [inventoryQuery.data, drugMap]);

  const stats = useMemo(() => {
    const rows = recordsQuery.data ?? [];
    return {
      total: rows.length,
      detected: rows.filter((r) => r.status === 'Detected').length,
      quarantined: rows.filter((r) => r.status === 'Quarantined').length,
      totalQty: rows.reduce((s, r) => s + r.quantityDamaged, 0),
    };
  }, [recordsQuery.data]);

  const handleCreate = async () => {
    const values = await form.validateFields();
    try {
      await createRecord.mutateAsync({
        ...values,
        damagedAt: values.damagedAt.toISOString(),
        status: 'Detected',
      });
      message.success('Damage record created.');
      form.resetFields();
      setModalOpen(false);
    } catch {
      message.error('Failed to create damage record.');
    }
  };

  const handleDelete = async (id: string) => {
    try { await deleteRecord.mutateAsync(id); message.success('Record deleted.'); }
    catch { message.error('Failed to delete.'); }
  };

  const columns: ColumnsType<DamageRecord> = [
    {
      title: 'Drug / Batch',
      dataIndex: 'drugInventoryId',
      render: (id: string) => {
        const inv = invMap.get(id);
        if (!inv) return id.slice(0, 8);
        return `${drugMap.get(inv.drugId) ?? inv.drugId} — ${inv.batchNumber ?? '?'}`;
      },
    },
    {
      title: 'Qty Damaged',
      dataIndex: 'quantityDamaged',
      align: 'right',
      width: 110,
      render: (v: number) => <Tag color="red">{v}</Tag>,
      sorter: (a, b) => a.quantityDamaged - b.quantityDamaged,
    },
    {
      title: 'Type',
      dataIndex: 'damageType',
      width: 120,
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Status',
      dataIndex: 'status',
      width: 110,
      render: (v: string | null) => <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    {
      title: 'Damaged At',
      dataIndex: 'damagedAt',
      width: 140,
      render: (v: string) => dayjs(v).format('DD MMM YYYY'),
      sorter: (a, b) => dayjs(a.damagedAt).unix() - dayjs(b.damagedAt).unix(),
      defaultSortOrder: 'descend',
    },
    {
      title: 'Discovered By',
      dataIndex: 'discoveredBy',
      width: 130,
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Notes',
      dataIndex: 'notes',
      ellipsis: true,
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 60,
      render: (_: unknown, record: DamageRecord) => (
        <Popconfirm title="Delete this record?" onConfirm={() => handleDelete(record.id)} okText="Delete" okType="danger">
          <Button type="text" size="small" danger icon={<DeleteOutlined />} />
        </Popconfirm>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col><Title level={4} style={{ margin: 0 }}>Damage Records</Title></Col>
        <Col>
          <Space>
            <Select placeholder="All Statuses" options={Object.keys(STATUS_COLORS).map((k) => ({ value: k, label: k }))}
              value={statusFilter} onChange={setStatusFilter} allowClear style={{ width: 160 }} />
            <Button icon={<ReloadOutlined />} onClick={() => { setStatusFilter(undefined); recordsQuery.refetch(); }} />
            <Button type="primary" icon={<PlusOutlined />} onClick={() => setModalOpen(true)}>
              Report Damage
            </Button>
          </Space>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        {[
          { label: 'Total Records', value: stats.total },
          { label: 'Detected', value: stats.detected, color: '#d46b08' },
          { label: 'Quarantined', value: stats.quarantined, color: '#faad14' },
          { label: 'Total Units Damaged', value: stats.totalQty, color: '#cf1322' },
        ].map((s) => (
          <Col xs={12} sm={6} key={s.label}>
            <Card size="small">
              <div style={{ fontSize: 12, color: '#888' }}>{s.label}</div>
              <div style={{ fontSize: 22, fontWeight: 700, color: s.color }}>{s.value}</div>
            </Card>
          </Col>
        ))}
      </Row>

      <Table<DamageRecord>
        rowKey="id"
        columns={columns}
        dataSource={recordsQuery.data ?? []}
        loading={recordsQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} records` }}
        scroll={{ x: 900 }}
      />

      <Modal
        title="Report Damage"
        open={modalOpen}
        onOk={handleCreate}
        onCancel={() => { form.resetFields(); setModalOpen(false); }}
        okText="Report"
        confirmLoading={createRecord.isPending}
        destroyOnClose
      >
        <Form form={form} layout="vertical" style={{ marginTop: 16 }}>
          <Form.Item name="drugInventoryId" label="Inventory Batch" rules={[{ required: true }]}>
            <Select showSearch options={invOptions} placeholder="Select batch"
              filterOption={(i, o) => (o?.label ?? '').toLowerCase().includes(i.toLowerCase())} />
          </Form.Item>
          <Form.Item name="quantityDamaged" label="Quantity Damaged" rules={[{ required: true }]}>
            <InputNumber min={1} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="damageType" label="Damage Type" rules={[{ required: true }]}>
            <Select options={DAMAGE_TYPES.map((t) => ({ value: t, label: t }))} />
          </Form.Item>
          <Form.Item name="damagedAt" label="Damaged At" rules={[{ required: true }]}
            initialValue={dayjs()}>
            <DatePicker style={{ width: '100%' }} format="DD MMM YYYY" />
          </Form.Item>
          <Form.Item name="discoveredBy" label="Discovered By" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="notes" label="Notes">
            <Input.TextArea rows={2} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
