import { useState, useEffect } from 'react';
import {
  Modal, Form, Select, Input, DatePicker, InputNumber,
  Table, Button, Space, Typography, Divider, message,
} from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import { useVendors } from '@/hooks/useVendors';
import { useDrugs } from '@/hooks/useDrugs';
import { useCreatePurchaseOrder, useCreatePurchaseOrderItem } from '@/hooks/usePurchaseOrderMutations';
import { useGlobalStore } from '@/store/globalStore';

const { Text } = Typography;

interface LineItem {
  key: string;
  drugId: string;
  quantityOrdered: number;
  unitPrice: number;
  batchNumber: string;
  expirationDate: dayjs.Dayjs | null;
}

interface Props {
  open: boolean;
  onClose: () => void;
}

type HeaderValues = {
  vendorId: string;
  poNumber?: string;
  orderDate: dayjs.Dayjs;
  notes?: string;
};

export default function PurchaseOrderFormModal({ open, onClose }: Props) {
  const [form] = Form.useForm<HeaderValues>();
  const [items, setItems] = useState<LineItem[]>([]);
  const currentUser = useGlobalStore((s) => s.currentUser);

  const vendorsQuery = useVendors();
  const drugsQuery = useDrugs();

  const createPO = useCreatePurchaseOrder();
  const createItem = useCreatePurchaseOrderItem();

  const vendorOptions = (vendorsQuery.data ?? [])
    .filter((v) => v.isActive)
    .map((v) => ({ value: v.id!, label: v.name ?? v.id }));

  const drugOptions = (drugsQuery.data ?? [])
    .filter((d) => d.isActive)
    .map((d) => ({ value: d.id!, label: `${d.name ?? d.id}${d.strength ? ` — ${d.strength}` : ''}` }));

  useEffect(() => {
    if (open) {
      form.setFieldsValue({ orderDate: dayjs() });
      setItems([]);
    }
  }, [open, form]);

  const addLine = () => {
    setItems((prev) => [
      ...prev,
      { key: crypto.randomUUID(), drugId: '', quantityOrdered: 1, unitPrice: 0, batchNumber: '', expirationDate: null },
    ]);
  };

  const removeLine = (key: string) => setItems((prev) => prev.filter((i) => i.key !== key));

  const updateLine = (key: string, field: keyof LineItem, value: unknown) => {
    setItems((prev) => prev.map((i) => (i.key === key ? { ...i, [field]: value } : i)));
  };

  const totalAmount = items.reduce((s, i) => s + (i.quantityOrdered ?? 0) * (i.unitPrice ?? 0), 0);

  const handleOk = async () => {
    const header = await form.validateFields();

    if (items.length === 0) {
      message.warning('Add at least one line item.');
      return;
    }

    const invalidLine = items.find((i) => !i.drugId || i.quantityOrdered <= 0);
    if (invalidLine) {
      message.warning('Each line item must have a drug and quantity > 0.');
      return;
    }

    try {
      const po = await createPO.mutateAsync({
        vendorId: header.vendorId,
        poNumber: header.poNumber,
        orderDate: header.orderDate.toISOString(),
        notes: header.notes,
        status: 'Draft',
        totalAmount,
        updatedBy: currentUser?.username ?? 'system',
      });

      if (po?.id) {
        await Promise.all(
          items.map((item) =>
            createItem.mutateAsync({
              purchaseOrderId: po.id,
              drugId: item.drugId,
              quantityOrdered: item.quantityOrdered,
              quantityReceived: 0,
              unitPrice: item.unitPrice,
              batchNumber: item.batchNumber || null,
              expirationDate: item.expirationDate?.toISOString() ?? null,
            }),
          ),
        );
      }

      message.success('Purchase order created.');
      form.resetFields();
      setItems([]);
      onClose();
    } catch {
      message.error('Failed to create purchase order.');
    }
  };

  const lineColumns = [
    {
      title: 'Drug',
      key: 'drug',
      width: 220,
      render: (_: unknown, row: LineItem) => (
        <Select
          showSearch
          placeholder="Select drug"
          options={drugOptions}
          value={row.drugId || undefined}
          onChange={(v) => updateLine(row.key, 'drugId', v)}
          filterOption={(i, o) => (o?.label ?? '').toLowerCase().includes(i.toLowerCase())}
          style={{ width: '100%' }}
          size="small"
        />
      ),
    },
    {
      title: 'Qty',
      key: 'qty',
      width: 80,
      render: (_: unknown, row: LineItem) => (
        <InputNumber
          min={1}
          value={row.quantityOrdered}
          onChange={(v) => updateLine(row.key, 'quantityOrdered', v ?? 1)}
          style={{ width: '100%' }}
          size="small"
        />
      ),
    },
    {
      title: 'Unit Price (₹)',
      key: 'price',
      width: 110,
      render: (_: unknown, row: LineItem) => (
        <InputNumber
          min={0}
          precision={2}
          value={row.unitPrice}
          onChange={(v) => updateLine(row.key, 'unitPrice', v ?? 0)}
          style={{ width: '100%' }}
          size="small"
        />
      ),
    },
    {
      title: 'Batch',
      key: 'batch',
      width: 110,
      render: (_: unknown, row: LineItem) => (
        <Input
          value={row.batchNumber}
          onChange={(e) => updateLine(row.key, 'batchNumber', e.target.value)}
          placeholder="Optional"
          size="small"
        />
      ),
    },
    {
      title: 'Expiry',
      key: 'expiry',
      width: 130,
      render: (_: unknown, row: LineItem) => (
        <DatePicker
          value={row.expirationDate}
          onChange={(v) => updateLine(row.key, 'expirationDate', v)}
          format="DD MMM YYYY"
          style={{ width: '100%' }}
          size="small"
        />
      ),
    },
    {
      title: 'Amount',
      key: 'amount',
      width: 90,
      align: 'right' as const,
      render: (_: unknown, row: LineItem) => (
        <Text>₹{((row.quantityOrdered ?? 0) * (row.unitPrice ?? 0)).toLocaleString('en-IN', { minimumFractionDigits: 2 })}</Text>
      ),
    },
    {
      title: '',
      key: 'del',
      width: 40,
      render: (_: unknown, row: LineItem) => (
        <Button type="text" danger size="small" icon={<DeleteOutlined />} onClick={() => removeLine(row.key)} />
      ),
    },
  ];

  return (
    <Modal
      title="New Purchase Order"
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); setItems([]); onClose(); }}
      okText="Create PO"
      confirmLoading={createPO.isPending || createItem.isPending}
      width={860}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Space direction="horizontal" size={16} style={{ width: '100%', display: 'flex' }}>
          <Form.Item name="vendorId" label="Vendor" rules={[{ required: true, message: 'Required' }]} style={{ flex: 2, marginBottom: 0 }}>
            <Select
              showSearch
              placeholder="Select vendor"
              options={vendorOptions}
              loading={vendorsQuery.isLoading}
              filterOption={(i, o) => (o?.label ?? '').toLowerCase().includes(i.toLowerCase())}
            />
          </Form.Item>
          <Form.Item name="poNumber" label="PO Number" style={{ flex: 1, marginBottom: 0 }}>
            <Input placeholder="e.g. PO-2026-001" />
          </Form.Item>
          <Form.Item name="orderDate" label="Order Date" rules={[{ required: true }]} style={{ flex: 1, marginBottom: 0 }}>
            <DatePicker style={{ width: '100%' }} format="DD MMM YYYY" />
          </Form.Item>
        </Space>
        <Form.Item name="notes" label="Notes" style={{ marginTop: 12, marginBottom: 0 }}>
          <Input.TextArea rows={2} placeholder="Optional notes" />
        </Form.Item>
      </Form>

      <Divider orientation="left" style={{ marginTop: 16 }}>Line Items</Divider>

      <Table
        dataSource={items}
        columns={lineColumns}
        rowKey="key"
        pagination={false}
        size="small"
        locale={{ emptyText: 'No items yet — click Add Item below' }}
        scroll={{ x: 700 }}
      />

      <Space style={{ marginTop: 12, width: '100%', justifyContent: 'space-between' }}>
        <Button icon={<PlusOutlined />} onClick={addLine}>Add Item</Button>
        <Text strong style={{ fontSize: 15 }}>
          Total: ₹{totalAmount.toLocaleString('en-IN', { minimumFractionDigits: 2 })}
        </Text>
      </Space>
    </Modal>
  );
}
