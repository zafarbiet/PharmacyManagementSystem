import { useState } from 'react';
import {
  Table, Button, Tag, Space, Typography, Row, Col, Card,
  Popconfirm, message, Select, Drawer, Descriptions, Divider,
  Tooltip,
} from 'antd';
import {
  PlusOutlined, ReloadOutlined, EditOutlined, DeleteOutlined,
  DollarOutlined, BellOutlined,
} from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { DebtRecord, DebtPayment, DebtReminder } from '@/api/localTypes';
import { useDebtRecords } from '@/hooks/useDebtRecords';
import { useCreateDebtRecord, useUpdateDebtRecord, useDeleteDebtRecord } from '@/hooks/useDebtRecordMutations';
import { useDebtPayments } from '@/hooks/useDebtPayments';
import { useCreateDebtPayment, useDeleteDebtPayment } from '@/hooks/useDebtPaymentMutations';
import { useDebtReminders } from '@/hooks/useDebtReminders';
import { useCreateDebtReminder, useDeleteDebtReminder } from '@/hooks/useDebtReminderMutations';
import { usePatients } from '@/hooks/usePatients';
import DebtRecordFormModal from '@/components/DebtRecordFormModal';
import DebtPaymentFormModal from '@/components/DebtPaymentFormModal';
import DebtReminderFormModal from '@/components/DebtReminderFormModal';

const { Title, Text } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Open: 'red',
  Partial: 'orange',
  Paid: 'green',
  'Written Off': 'default',
};

function fmt(n: number) {
  return `₹${n.toLocaleString('en-IN', { minimumFractionDigits: 2 })}`;
}

function DebtDetailDrawer({
  record,
  onClose,
}: {
  record: DebtRecord | null;
  onClose: () => void;
}) {
  const [paymentModalOpen, setPaymentModalOpen] = useState(false);
  const [reminderModalOpen, setReminderModalOpen] = useState(false);

  const paymentsQuery = useDebtPayments(record?.id);
  const remindersQuery = useDebtReminders(record?.id);
  const createPayment = useCreateDebtPayment();
  const deletePayment = useDeleteDebtPayment();
  const createReminder = useCreateDebtReminder();
  const deleteReminder = useDeleteDebtReminder();

  const payments: DebtPayment[] = paymentsQuery.data ?? [];
  const reminders: DebtReminder[] = remindersQuery.data ?? [];

  if (!record) return null;

  const paymentCols: ColumnsType<DebtPayment> = [
    {
      title: 'Date',
      dataIndex: 'paymentDate',
      render: (v: string) => dayjs(v).format('DD/MM/YYYY'),
      width: 100,
    },
    {
      title: 'Amount',
      dataIndex: 'amountPaid',
      align: 'right',
      render: (v: number) => fmt(v),
    },
    {
      title: 'Method',
      dataIndex: 'paymentMethod',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Received By',
      dataIndex: 'receivedBy',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: '',
      key: 'del',
      width: 40,
      render: (_: unknown, row: DebtPayment) => (
        <Popconfirm
          title="Delete this payment?"
          onConfirm={async () => {
            try {
              await deletePayment.mutateAsync(row.id);
              message.success('Payment deleted.');
            } catch {
              message.error('Failed to delete payment.');
            }
          }}
          okText="Delete"
          okType="danger"
        >
          <Button type="text" size="small" danger icon={<DeleteOutlined />} />
        </Popconfirm>
      ),
    },
  ];

  const reminderCols: ColumnsType<DebtReminder> = [
    {
      title: 'Sent At',
      dataIndex: 'sentAt',
      render: (v: string) => dayjs(v).format('DD/MM/YYYY HH:mm'),
      width: 130,
    },
    { title: 'Channel', dataIndex: 'channel', render: (v: string | null) => v ?? '—' },
    { title: 'Sent By', dataIndex: 'sentBy', render: (v: string | null) => v ?? '—' },
    { title: 'Message', dataIndex: 'message', render: (v: string | null) => v ?? '—', ellipsis: true },
    {
      title: '',
      key: 'del',
      width: 40,
      render: (_: unknown, row: DebtReminder) => (
        <Popconfirm
          title="Delete this reminder log?"
          onConfirm={async () => {
            try {
              await deleteReminder.mutateAsync(row.id);
              message.success('Reminder deleted.');
            } catch {
              message.error('Failed to delete reminder.');
            }
          }}
          okText="Delete"
          okType="danger"
        >
          <Button type="text" size="small" danger icon={<DeleteOutlined />} />
        </Popconfirm>
      ),
    },
  ];

  return (
    <>
      <Drawer
        title={`Debt Detail — ${fmt(record.originalAmount)}`}
        open={!!record}
        onClose={onClose}
        width={640}
      >
        <Descriptions size="small" bordered column={2} style={{ marginBottom: 16 }}>
          <Descriptions.Item label="Original">{fmt(record.originalAmount)}</Descriptions.Item>
          <Descriptions.Item label="Remaining">
            <Text type={record.remainingAmount > 0 ? 'danger' : 'success'}>
              {fmt(record.remainingAmount)}
            </Text>
          </Descriptions.Item>
          <Descriptions.Item label="Due Date">
            {record.dueDate ? dayjs(record.dueDate).format('DD MMM YYYY') : '—'}
          </Descriptions.Item>
          <Descriptions.Item label="Status">
            <Tag color={STATUS_COLORS[record.status ?? ''] ?? 'default'}>{record.status ?? '—'}</Tag>
          </Descriptions.Item>
          {record.notes && (
            <Descriptions.Item label="Notes" span={2}>{record.notes}</Descriptions.Item>
          )}
        </Descriptions>

        <Divider orientation="left" style={{ marginTop: 0 }}>
          Payments
          <Button
            size="small"
            type="link"
            icon={<PlusOutlined />}
            onClick={() => setPaymentModalOpen(true)}
            disabled={record.remainingAmount <= 0}
            style={{ marginLeft: 8 }}
          >
            Add
          </Button>
        </Divider>
        <Table
          dataSource={payments}
          columns={paymentCols}
          rowKey="id"
          size="small"
          pagination={false}
          loading={paymentsQuery.isFetching}
        />

        <Divider orientation="left">
          Reminders
          <Button
            size="small"
            type="link"
            icon={<PlusOutlined />}
            onClick={() => setReminderModalOpen(true)}
            style={{ marginLeft: 8 }}
          >
            Log
          </Button>
        </Divider>
        <Table
          dataSource={reminders}
          columns={reminderCols}
          rowKey="id"
          size="small"
          pagination={false}
          loading={remindersQuery.isFetching}
        />
      </Drawer>

      <DebtPaymentFormModal
        open={paymentModalOpen}
        debtRecordId={record.id}
        maxAmount={record.remainingAmount}
        onClose={() => setPaymentModalOpen(false)}
        onSubmit={async (values) => {
          await createPayment.mutateAsync(values);
          message.success('Payment recorded.');
          setPaymentModalOpen(false);
        }}
        loading={createPayment.isPending}
      />

      <DebtReminderFormModal
        open={reminderModalOpen}
        debtRecordId={record.id}
        onClose={() => setReminderModalOpen(false)}
        onSubmit={async (values) => {
          await createReminder.mutateAsync(values);
          message.success('Reminder logged.');
          setReminderModalOpen(false);
        }}
        loading={createReminder.isPending}
      />
    </>
  );
}

export default function DebtRecordsListPage() {
  const [statusFilter, setStatusFilter] = useState<string | undefined>(undefined);
  const [modalState, setModalState] = useState<{ open: boolean; record: DebtRecord | null }>({
    open: false,
    record: null,
  });
  const [drawerRecord, setDrawerRecord] = useState<DebtRecord | null>(null);

  const debtQuery = useDebtRecords(undefined, statusFilter);
  const patientsQuery = usePatients();
  const createDebt = useCreateDebtRecord();
  const updateDebt = useUpdateDebtRecord();
  const deleteDebt = useDeleteDebtRecord();

  const records: DebtRecord[] = debtQuery.data ?? [];
  const patients = patientsQuery.data ?? [];

  const patientMap: Record<string, string> = {};
  patients.forEach((p) => { if (p.id) patientMap[p.id] = p.name ?? p.id; });

  const totalDebt = records.reduce((s, r) => s + r.remainingAmount, 0);
  const openCount = records.filter((r) => r.status === 'Open').length;
  const overdueCount = records.filter(
    (r) => r.dueDate && dayjs(r.dueDate).isBefore(dayjs(), 'day') && r.remainingAmount > 0,
  ).length;

  const handleSubmit = async (values: Partial<DebtRecord>) => {
    try {
      if (modalState.record) {
        await updateDebt.mutateAsync({ id: modalState.record.id, ...values });
        message.success('Debt record updated.');
      } else {
        await createDebt.mutateAsync(values);
        message.success('Debt record created.');
      }
      setModalState({ open: false, record: null });
    } catch {
      message.error('Failed to save debt record.');
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteDebt.mutateAsync(id);
      message.success('Debt record deleted.');
    } catch {
      message.error('Failed to delete debt record.');
    }
  };

  const columns: ColumnsType<DebtRecord> = [
    {
      title: 'Patient',
      dataIndex: 'patientId',
      render: (v: string) => patientMap[v] ?? v,
      sorter: (a, b) => (patientMap[a.patientId] ?? '').localeCompare(patientMap[b.patientId] ?? ''),
    },
    {
      title: 'Original',
      dataIndex: 'originalAmount',
      align: 'right',
      render: (v: number) => fmt(v),
      sorter: (a, b) => a.originalAmount - b.originalAmount,
    },
    {
      title: 'Remaining',
      dataIndex: 'remainingAmount',
      align: 'right',
      render: (v: number) => (
        <Text type={v > 0 ? 'danger' : 'success'}>{fmt(v)}</Text>
      ),
      sorter: (a, b) => a.remainingAmount - b.remainingAmount,
    },
    {
      title: 'Due Date',
      dataIndex: 'dueDate',
      render: (v: string | null) => {
        if (!v) return <span style={{ color: '#bbb' }}>—</span>;
        const d = dayjs(v);
        const overdue = d.isBefore(dayjs(), 'day');
        return <span style={{ color: overdue ? '#cf1322' : undefined }}>{d.format('DD MMM YYYY')}</span>;
      },
      sorter: (a, b) => (a.dueDate ?? '').localeCompare(b.dueDate ?? ''),
    },
    {
      title: 'Status',
      dataIndex: 'status',
      render: (v: string | null) => (
        <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>
      ),
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 130,
      render: (_: unknown, record: DebtRecord) => (
        <Space size={4}>
          <Tooltip title="Payments & Reminders">
            <Button
              size="small"
              icon={<DollarOutlined />}
              onClick={() => setDrawerRecord(record)}
            />
          </Tooltip>
          <Tooltip title="Log Reminder">
            <Button
              size="small"
              icon={<BellOutlined />}
              onClick={() => setDrawerRecord(record)}
            />
          </Tooltip>
          <Button
            size="small"
            icon={<EditOutlined />}
            onClick={() => setModalState({ open: true, record })}
          />
          <Popconfirm
            title="Delete this debt record?"
            onConfirm={() => handleDelete(record.id)}
            okText="Delete"
            okType="danger"
          >
            <Button type="text" size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>Debt Records</Title>
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => setModalState({ open: true, record: null })}
          >
            New Debt Record
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Outstanding</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: totalDebt > 0 ? '#cf1322' : '#888' }}>
              {fmt(totalDebt)}
            </div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Open Debts</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{openCount}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Overdue</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: overdueCount > 0 ? '#cf1322' : '#888' }}>
              {overdueCount}
            </div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col>
          <Select
            placeholder="Filter by status"
            allowClear
            style={{ width: 180 }}
            value={statusFilter}
            onChange={(v) => setStatusFilter(v)}
          >
            <Select.Option value="Open">Open</Select.Option>
            <Select.Option value="Partial">Partial</Select.Option>
            <Select.Option value="Paid">Paid</Select.Option>
            <Select.Option value="Written Off">Written Off</Select.Option>
          </Select>
        </Col>
        <Col>
          <Button icon={<ReloadOutlined />} onClick={() => debtQuery.refetch()} />
        </Col>
      </Row>

      <Table<DebtRecord>
        rowKey="id"
        columns={columns}
        dataSource={records}
        loading={debtQuery.isFetching}
        size="small"
        pagination={{ pageSize: 25, showSizeChanger: true, showTotal: (t) => `${t} records` }}
        scroll={{ x: 800 }}
      />

      <DebtRecordFormModal
        open={modalState.open}
        record={modalState.record}
        patients={patients}
        onClose={() => setModalState({ open: false, record: null })}
        onSubmit={handleSubmit}
        loading={createDebt.isPending || updateDebt.isPending}
      />

      <DebtDetailDrawer
        record={drawerRecord}
        onClose={() => setDrawerRecord(null)}
      />
    </div>
  );
}
