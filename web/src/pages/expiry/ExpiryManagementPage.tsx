import { useState, useMemo } from 'react';
import {
  Table, Select, Button, Tag, Space, Typography, Row, Col, Card,
  Tooltip, Popconfirm, Modal, Form, DatePicker, InputNumber, Input, message,
} from 'antd';
import { ReloadOutlined, WarningOutlined, PlusOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { ExpiryRecord } from '@/api/localTypes';
import { useExpiryRecords } from '@/hooks/useExpiryRecords';
import { useDrugInventory } from '@/hooks/useDrugInventory';
import { useDrugs } from '@/hooks/useDrugs';
import { useCreateExpiryRecord, useUpdateExpiryRecord, useDeleteExpiryRecord } from '@/hooks/useExpiryRecordMutations';
import { useGlobalStore } from '@/store/globalStore';

const { Title } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Detected: 'orange',
  Quarantined: 'gold',
  ReturnedToVendor: 'blue',
  Disposed: 'default',
  Pending: 'red',
};

const STATUS_OPTIONS = Object.keys(STATUS_COLORS).map((s) => ({ value: s, label: s }));

type LogFormValues = {
  drugInventoryId: string;
  expirationDate: dayjs.Dayjs;
  quantityAffected: number;
  notes?: string;
};

export default function ExpiryManagementPage() {
  const [statusFilter, setStatusFilter] = useState<string | undefined>();
  const [logModalOpen, setLogModalOpen] = useState(false);
  const [logForm] = Form.useForm<LogFormValues>();

  const currentUser = useGlobalStore((s) => s.currentUser);
  const username = currentUser?.username ?? 'system';

  const expiryQuery = useExpiryRecords();
  const inventoryQuery = useDrugInventory();
  const drugsQuery = useDrugs();

  const createRecord = useCreateExpiryRecord();
  const updateRecord = useUpdateExpiryRecord();
  const deleteRecord = useDeleteExpiryRecord();

  const inventoryMap = useMemo(() => {
    const map = new Map<string, string>(); // inventoryId → drugId
    for (const inv of inventoryQuery.data ?? []) {
      map.set(inv.id, inv.drugId);
    }
    return map;
  }, [inventoryQuery.data]);

  const drugMap = useMemo(() => {
    const map = new Map<string, string>(); // drugId → name
    for (const d of drugsQuery.data ?? []) {
      if (d.id) map.set(d.id, d.name ?? d.id);
    }
    return map;
  }, [drugsQuery.data]);

  const inventoryOptions = useMemo(
    () =>
      (inventoryQuery.data ?? [])
        .filter((inv) => inv.isActive)
        .map((inv) => {
          const drugName = drugMap.get(inv.drugId) ?? inv.drugId;
          const label = `${drugName}${inv.batchNumber ? ` — Batch ${inv.batchNumber}` : ''} (Qty: ${inv.quantityInStock})`;
          return { value: inv.id, label };
        }),
    [inventoryQuery.data, drugMap],
  );

  const filtered = useMemo(() => {
    let rows = (expiryQuery.data ?? []) as ExpiryRecord[];
    if (statusFilter) rows = rows.filter((r) => r.status === statusFilter);
    return rows;
  }, [expiryQuery.data, statusFilter]);

  const getDrugName = (drugInventoryId: string) => {
    const drugId = inventoryMap.get(drugInventoryId);
    return drugId ? (drugMap.get(drugId) ?? '—') : '—';
  };

  const detectedCount = filtered.filter((r) => r.status === 'Detected' || r.status === 'Pending').length;
  const quarantinedCount = filtered.filter((r) => r.status === 'Quarantined').length;
  const totalQty = filtered.reduce((s, r) => s + r.quantityAffected, 0);

  const transition = async (record: ExpiryRecord, newStatus: string, extraFields?: Partial<ExpiryRecord>) => {
    try {
      await updateRecord.mutateAsync({
        id: record.id,
        drugInventoryId: record.drugInventoryId,
        detectedAt: record.detectedAt,
        expirationDate: record.expirationDate,
        quantityAffected: record.quantityAffected,
        initiatedBy: record.initiatedBy,
        notes: record.notes,
        status: newStatus,
        ...extraFields,
      });
      message.success(`Status updated to ${newStatus}.`);
    } catch {
      message.error('Failed to update status.');
    }
  };

  const handleLogExpiry = async () => {
    const values = await logForm.validateFields();
    try {
      await createRecord.mutateAsync({
        drugInventoryId: values.drugInventoryId,
        detectedAt: new Date().toISOString(),
        expirationDate: values.expirationDate.toISOString(),
        quantityAffected: values.quantityAffected,
        status: 'Detected',
        initiatedBy: username,
        notes: values.notes || null,
      });
      message.success('Expiry record logged.');
      logForm.resetFields();
      setLogModalOpen(false);
    } catch {
      message.error('Failed to log expiry record.');
    }
  };

  const columns: ColumnsType<ExpiryRecord> = [
    {
      title: 'Drug',
      dataIndex: 'drugInventoryId',
      key: 'drug',
      render: (id: string) => getDrugName(id),
      sorter: (a, b) => getDrugName(a.drugInventoryId).localeCompare(getDrugName(b.drugInventoryId)),
    },
    {
      title: 'Expiration Date',
      dataIndex: 'expirationDate',
      key: 'expirationDate',
      render: (v: string) => {
        const d = dayjs(v);
        const isExpired = d.isBefore(dayjs());
        return (
          <Tooltip title={d.format('DD MMM YYYY')}>
            <Tag color={isExpired ? 'red' : 'orange'}>{d.format('DD MMM YYYY')}</Tag>
          </Tooltip>
        );
      },
      sorter: (a, b) => dayjs(a.expirationDate).unix() - dayjs(b.expirationDate).unix(),
      defaultSortOrder: 'ascend',
    },
    {
      title: 'Detected At',
      dataIndex: 'detectedAt',
      key: 'detectedAt',
      render: (v: string) => dayjs(v).format('DD MMM YYYY'),
      sorter: (a, b) => dayjs(a.detectedAt).unix() - dayjs(b.detectedAt).unix(),
    },
    {
      title: 'Qty',
      dataIndex: 'quantityAffected',
      key: 'quantityAffected',
      align: 'right',
      sorter: (a, b) => a.quantityAffected - b.quantityAffected,
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: (v: string | null) => (
        <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>
      ),
    },
    {
      title: 'Initiated By',
      dataIndex: 'initiatedBy',
      key: 'initiatedBy',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Approved By',
      dataIndex: 'approvedBy',
      key: 'approvedBy',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Notes',
      dataIndex: 'notes',
      key: 'notes',
      ellipsis: true,
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 150,
      render: (_: unknown, record: ExpiryRecord) => {
        const status = record.status ?? '';
        return (
          <Space size={4} wrap>
            {(status === 'Detected' || status === 'Pending') && (
              <Popconfirm
                title="Quarantine this batch?"
                onConfirm={() => transition(record, 'Quarantined', { approvedBy: username, approvedAt: new Date().toISOString() })}
                okText="Quarantine"
              >
                <Button size="small" type="default">Quarantine</Button>
              </Popconfirm>
            )}
            {status === 'Quarantined' && (
              <>
                <Popconfirm
                  title="Mark as disposed?"
                  onConfirm={() => transition(record, 'Disposed', { approvedBy: username, approvedAt: new Date().toISOString() })}
                  okText="Dispose"
                  okType="danger"
                >
                  <Button size="small" danger>Dispose</Button>
                </Popconfirm>
                <Popconfirm
                  title="Return to vendor?"
                  onConfirm={() => transition(record, 'ReturnedToVendor', { approvedBy: username, approvedAt: new Date().toISOString() })}
                  okText="Return"
                >
                  <Button size="small" type="dashed">Return</Button>
                </Popconfirm>
              </>
            )}
            {(status === 'Disposed' || status === 'ReturnedToVendor') && (
              <Popconfirm
                title="Delete this record?"
                onConfirm={async () => {
                  try { await deleteRecord.mutateAsync(record.id); message.success('Record deleted.'); }
                  catch { message.error('Failed to delete.'); }
                }}
                okType="danger"
                okText="Delete"
              >
                <Button size="small" type="text" danger>Delete</Button>
              </Popconfirm>
            )}
          </Space>
        );
      },
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            <WarningOutlined style={{ color: '#faad14', marginRight: 8 }} />
            Expiry Management
          </Title>
        </Col>
        <Col>
          <Button type="primary" icon={<PlusOutlined />} onClick={() => { logForm.resetFields(); setLogModalOpen(true); }}>
            Log Expiry
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Records</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{filtered.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Detected / Pending</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: detectedCount > 0 ? '#cf1322' : '#3f8600' }}>
              {detectedCount}
            </div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Quarantined</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: quarantinedCount > 0 ? '#d46b08' : '#3f8600' }}>
              {quarantinedCount}
            </div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col style={{ width: 200 }}>
          <Select
            placeholder="All Statuses"
            options={STATUS_OPTIONS}
            value={statusFilter}
            onChange={setStatusFilter}
            allowClear
            style={{ width: '100%' }}
          />
        </Col>
        <Col>
          <Button
            icon={<ReloadOutlined />}
            onClick={() => { setStatusFilter(undefined); expiryQuery.refetch(); }}
          />
        </Col>
        <Col>
          <span style={{ color: '#888', lineHeight: '32px' }}>
            Total affected qty: <strong>{totalQty.toLocaleString('en-IN')}</strong>
          </span>
        </Col>
      </Row>

      <Table<ExpiryRecord>
        rowKey="id"
        columns={columns}
        dataSource={filtered}
        loading={expiryQuery.isFetching || inventoryQuery.isFetching || drugsQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} records` }}
        scroll={{ x: 1050 }}
        rowClassName={(r) => {
          const status = r.status ?? '';
          if (status === 'Detected' || status === 'Pending') return 'row-alert';
          return '';
        }}
      />

      {/* Log Expiry Modal */}
      <Modal
        title="Log Expiry Record"
        open={logModalOpen}
        onOk={handleLogExpiry}
        onCancel={() => { logForm.resetFields(); setLogModalOpen(false); }}
        okText="Log Expiry"
        confirmLoading={createRecord.isPending}
        width={520}
        destroyOnClose
      >
        <Form form={logForm} layout="vertical" size="middle" style={{ marginTop: 16 }}>
          <Form.Item
            name="drugInventoryId"
            label="Drug / Batch"
            rules={[{ required: true, message: 'Required' }]}
          >
            <Select
              showSearch
              placeholder="Select drug batch"
              options={inventoryOptions}
              loading={inventoryQuery.isLoading}
              filterOption={(i, o) => (o?.label ?? '').toLowerCase().includes(i.toLowerCase())}
            />
          </Form.Item>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                name="expirationDate"
                label="Expiration Date"
                rules={[{ required: true }]}
              >
                <DatePicker style={{ width: '100%' }} format="DD MMM YYYY" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item
                name="quantityAffected"
                label="Qty Affected"
                rules={[{ required: true }]}
              >
                <InputNumber min={1} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
          </Row>
          <Form.Item name="notes" label="Notes">
            <Input.TextArea rows={2} placeholder="Optional notes" />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
