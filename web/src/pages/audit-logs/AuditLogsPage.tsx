import { Table, Space, Typography, Tag } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import { useQuery } from '@tanstack/react-query';
import dayjs from 'dayjs';
import axiosClient from '@/api/axiosClient';

const { Title } = Typography;

type AuditLog = {
  id: string;
  drugId: string;
  drugName: string | null;
  scheduleCategory: string | null;
  customerInvoiceId: string;
  prescriptionId: string | null;
  patientId: string | null;
  quantityDispensed: number;
  performedBy: string | null;
  performedAt: string;
  retentionUntil: string;
};

function useAuditLogs() {
  return useQuery({
    queryKey: ['audit-logs'],
    queryFn: () => axiosClient.get<AuditLog[]>('/audit-logs').then((r) => r.data),
  });
}

export default function AuditLogsPage() {
  const { data, isLoading } = useAuditLogs();

  const columns: ColumnsType<AuditLog> = [
    { title: 'Performed At', dataIndex: 'performedAt', key: 'performedAt', render: (v: string) => dayjs(v).format('DD MMM YYYY HH:mm') },
    { title: 'Drug', dataIndex: 'drugName', key: 'drugName', render: (v: string | null) => v ?? '—' },
    {
      title: 'Schedule', dataIndex: 'scheduleCategory', key: 'scheduleCategory',
      render: (v: string | null) => v ? <Tag color="orange">{v}</Tag> : '—',
    },
    { title: 'Qty Dispensed', dataIndex: 'quantityDispensed', key: 'quantityDispensed' },
    { title: 'Performed By', dataIndex: 'performedBy', key: 'performedBy', render: (v: string | null) => v ?? '—' },
    { title: 'Invoice ID', dataIndex: 'customerInvoiceId', key: 'customerInvoiceId', render: (v: string) => v.slice(0, 8) + '…' },
    { title: 'Prescription', dataIndex: 'prescriptionId', key: 'prescriptionId', render: (v: string | null) => v ? v.slice(0, 8) + '…' : '—' },
    { title: 'Retention Until', dataIndex: 'retentionUntil', key: 'retentionUntil', render: (v: string) => dayjs(v).format('DD MMM YYYY') },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Space style={{ marginBottom: 16, justifyContent: 'space-between', width: '100%' }}>
        <Title level={3} style={{ margin: 0 }}>Audit Logs</Title>
      </Space>
      <Table rowKey="id" dataSource={data ?? []} columns={columns} loading={isLoading} size="small" />
    </div>
  );
}
