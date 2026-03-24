import { useEffect, useState } from 'react';
import {
  Modal, Table, InputNumber, Input, DatePicker, Typography, Alert, Spin, Space, message,
} from 'antd';
import dayjs from 'dayjs';
import type { ColumnsType } from 'antd/es/table';
import type { PurchaseOrder } from '@/api/localTypes';
import { usePurchaseOrderItems } from '@/hooks/usePurchaseOrderItems';
import { useDrugs } from '@/hooks/useDrugs';
import { useReceiveConsignment } from '@/hooks/usePurchaseOrderMutations';

const { Text } = Typography;

interface RowData {
  drugId: string;
  drugName: string;
  quantityOrdered: number;
  quantityReceived: number;
  batchNumber: string;
  expirationDate: dayjs.Dayjs | null;
}

interface Props {
  purchaseOrder: PurchaseOrder | null;
  onClose: () => void;
}

export default function ReceiveConsignmentModal({ purchaseOrder, onClose }: Props) {
  const open = !!purchaseOrder;
  const itemsQuery = usePurchaseOrderItems(purchaseOrder?.id);
  const drugsQuery = useDrugs();
  const receive = useReceiveConsignment();

  const drugMap = new Map(
    (drugsQuery.data ?? []).map((d) => [d.id ?? '', d.name ?? d.id ?? '']),
  );

  const [rows, setRows] = useState<RowData[]>([]);

  useEffect(() => {
    if (open && itemsQuery.data) {
      setRows(
        itemsQuery.data.map((item) => ({
          drugId: item.drugId,
          drugName: drugMap.get(item.drugId) ?? item.drugId,
          quantityOrdered: item.quantityOrdered,
          quantityReceived: item.quantityOrdered,
          batchNumber: item.batchNumber ?? '',
          expirationDate: item.expirationDate ? dayjs(item.expirationDate) : null,
        })),
      );
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open, itemsQuery.data]);

  const updateRow = (drugId: string, field: keyof RowData, value: unknown) => {
    setRows((prev) =>
      prev.map((r) => (r.drugId === drugId ? { ...r, [field]: value } : r)),
    );
  };

  const handleOk = async () => {
    const invalid = rows.find((r) => r.quantityReceived > 0 && (!r.batchNumber.trim() || !r.expirationDate));
    if (invalid) {
      message.warning(`Please enter batch number and expiry date for "${invalid.drugName}".`);
      return;
    }

    try {
      await receive.mutateAsync({
        id: purchaseOrder!.id,
        items: rows.map((r) => ({
          drugId: r.drugId,
          quantityReceived: r.quantityReceived,
          batchNumber: r.batchNumber || null,
          expirationDate: r.expirationDate?.toISOString() ?? null,
        })),
      });
      message.success('Consignment received. Inventory updated.');
      onClose();
    } catch {
      message.error('Failed to receive consignment.');
    }
  };

  const columns: ColumnsType<RowData> = [
    {
      title: 'Drug',
      dataIndex: 'drugName',
      key: 'drug',
    },
    {
      title: 'Ordered',
      dataIndex: 'quantityOrdered',
      align: 'right',
      width: 80,
    },
    {
      title: 'Received',
      dataIndex: 'quantityReceived',
      width: 100,
      render: (val: number, row: RowData) => (
        <InputNumber
          min={0}
          max={row.quantityOrdered}
          value={val}
          onChange={(v) => updateRow(row.drugId, 'quantityReceived', v ?? 0)}
          style={{ width: '100%' }}
          size="small"
        />
      ),
    },
    {
      title: 'Batch Number',
      dataIndex: 'batchNumber',
      width: 140,
      render: (val: string, row: RowData) => (
        <Input
          value={val}
          placeholder="e.g. BT-001"
          onChange={(e) => updateRow(row.drugId, 'batchNumber', e.target.value)}
          size="small"
        />
      ),
    },
    {
      title: 'Expiry Date',
      dataIndex: 'expirationDate',
      width: 150,
      render: (val: dayjs.Dayjs | null, row: RowData) => (
        <DatePicker
          value={val}
          format="DD MMM YYYY"
          onChange={(d) => updateRow(row.drugId, 'expirationDate', d)}
          size="small"
          style={{ width: '100%' }}
        />
      ),
    },
  ];

  return (
    <Modal
      title={`Receive Consignment — PO ${purchaseOrder?.poNumber ?? purchaseOrder?.id?.slice(0, 8)}`}
      open={open}
      onOk={handleOk}
      onCancel={onClose}
      okText="Confirm Receipt"
      confirmLoading={receive.isPending}
      width={820}
      destroyOnClose
    >
      <Alert
        type="info"
        showIcon
        message="Enter batch numbers and expiry dates for each received drug. Inventory will be auto-updated."
        style={{ marginBottom: 16 }}
      />

      {itemsQuery.isLoading ? (
        <Spin />
      ) : rows.length === 0 ? (
        <Text type="secondary">No items found on this purchase order.</Text>
      ) : (
        <Space direction="vertical" style={{ width: '100%' }}>
          <Table<RowData>
            rowKey="drugId"
            columns={columns}
            dataSource={rows}
            pagination={false}
            size="small"
            scroll={{ x: 700 }}
          />
        </Space>
      )}
    </Modal>
  );
}
