import { Table, Space, Typography, Tag } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import { useQuery } from '@tanstack/react-query';
import dayjs from 'dayjs';
import axiosClient from '@/api/axiosClient';

const { Title } = Typography;

type StockTransaction = {
  id: string;
  drugId: string;
  batchNumber: string | null;
  transactionType: string | null;
  quantity: number;
  transactionDate: string;
  referenceId: string | null;
  referenceType: string | null;
  notes: string | null;
};

type Drug = { id: string; name: string | null };

const TYPE_COLORS: Record<string, string> = {
  purchase: 'green',
  sale: 'blue',
  return: 'orange',
  adjustment: 'purple',
  damage: 'red',
  expiry: 'default',
};

function useStockTransactions() {
  return useQuery({
    queryKey: ['stock-transactions'],
    queryFn: () => axiosClient.get<StockTransaction[]>('/stock-transactions').then((r) => r.data),
  });
}

function useDrugs() {
  return useQuery({
    queryKey: ['drugs'],
    queryFn: () => axiosClient.get<Drug[]>('/drugs').then((r) => r.data),
    staleTime: 1000 * 60 * 5,
  });
}

export default function StockTransactionsPage() {
  const { data, isLoading } = useStockTransactions();
  const { data: drugs } = useDrugs();

  const drugMap = Object.fromEntries(drugs?.map((d) => [d.id, d.name ?? d.id]) ?? []);

  const columns: ColumnsType<StockTransaction> = [
    { title: 'Date', dataIndex: 'transactionDate', key: 'transactionDate', render: (v: string) => dayjs(v).format('DD MMM YYYY HH:mm') },
    { title: 'Drug', dataIndex: 'drugId', key: 'drugId', render: (v: string) => drugMap[v] ?? v },
    { title: 'Batch', dataIndex: 'batchNumber', key: 'batchNumber', render: (v: string | null) => v ?? '—' },
    {
      title: 'Type', dataIndex: 'transactionType', key: 'transactionType',
      render: (v: string | null) => <Tag color={TYPE_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    {
      title: 'Quantity', dataIndex: 'quantity', key: 'quantity',
      render: (v: number, row: StockTransaction) => {
        const isOut = ['sale', 'damage', 'expiry'].includes(row.transactionType ?? '');
        return <span style={{ color: isOut ? '#cf1322' : '#389e0d' }}>{isOut ? `-${v}` : `+${v}`}</span>;
      },
    },
    { title: 'Reference Type', dataIndex: 'referenceType', key: 'referenceType', render: (v: string | null) => v ?? '—' },
    { title: 'Notes', dataIndex: 'notes', key: 'notes', render: (v: string | null) => v ?? '—' },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Space style={{ marginBottom: 16, justifyContent: 'space-between', width: '100%' }}>
        <Title level={3} style={{ margin: 0 }}>Stock Transactions</Title>
      </Space>
      <Table rowKey="id" dataSource={data ?? []} columns={columns} loading={isLoading} size="small" />
    </div>
  );
}
