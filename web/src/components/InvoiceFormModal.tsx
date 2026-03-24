import { useState, useEffect, useMemo } from 'react';
import {
  Modal, Form, Select, DatePicker, InputNumber,
  Table, Button, Space, Typography, Divider, Input, message, Tag,
} from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import type { Drug } from '@/api/types.gen';
import { useDrugs } from '@/hooks/useDrugs';
import { useCreateCustomerInvoice } from '@/hooks/useCustomerInvoiceMutations';
import { useGlobalStore } from '@/store/globalStore';

const { Text } = Typography;

const PAYMENT_MODES = ['Cash', 'UPI', 'Card', 'Credit', 'NEFT'];
const PAYMENT_COLORS: Record<string, string> = {
  Cash: 'green', UPI: 'blue', Card: 'geekblue', Credit: 'orange', NEFT: 'cyan',
};

interface LineItem {
  key: string;
  drugId: string;
  batchNumber: string;
  quantity: number;
  unitPrice: number;
  discountPercent: number;
}

interface Props {
  open: boolean;
  onClose: () => void;
}

type HeaderValues = {
  invoiceDate: dayjs.Dayjs;
  paymentMethod: string;
  notes?: string;
};

function computeLineGst(unitPrice: number, qty: number, discPct: number, gstSlab: number) {
  const gross = unitPrice * qty;
  const discAmt = gross * (discPct / 100);
  const taxable = gross - discAmt;
  const cgst = taxable * (gstSlab / 2) / 100;
  const sgst = cgst;
  return { gross, discAmt, taxable, cgst, sgst, total: taxable + cgst + sgst };
}

export default function InvoiceFormModal({ open, onClose }: Props) {
  const [form] = Form.useForm<HeaderValues>();
  const [items, setItems] = useState<LineItem[]>([]);

  const currentUser = useGlobalStore((s) => s.currentUser);
  const selectedBranch = useGlobalStore((s) => s.selectedBranch);

  const drugsQuery = useDrugs();
  const createInvoice = useCreateCustomerInvoice();

  const drugMap = useMemo(() => {
    const m = new Map<string, Drug>();
    for (const d of drugsQuery.data ?? []) {
      if (d.id) m.set(d.id, d);
    }
    return m;
  }, [drugsQuery.data]);

  const drugOptions = useMemo(
    () =>
      (drugsQuery.data ?? [])
        .filter((d) => d.isActive)
        .map((d) => ({
          value: d.id!,
          label: `${d.name ?? d.id}${d.strength ? ` — ${d.strength}` : ''}`,
          mrp: d.mrp ?? 0,
          gstSlab: d.gstSlab ?? 0,
        })),
    [drugsQuery.data],
  );

  useEffect(() => {
    if (open) {
      form.setFieldsValue({ invoiceDate: dayjs(), paymentMethod: 'Cash' });
      setItems([]);
    }
  }, [open, form]);

  const addLine = () => {
    setItems((prev) => [
      ...prev,
      { key: crypto.randomUUID(), drugId: '', batchNumber: '', quantity: 1, unitPrice: 0, discountPercent: 0 },
    ]);
  };

  const removeLine = (key: string) => setItems((prev) => prev.filter((i) => i.key !== key));

  const updateLine = (key: string, field: keyof LineItem, value: unknown) => {
    setItems((prev) =>
      prev.map((i) => {
        if (i.key !== key) return i;
        const updated = { ...i, [field]: value };
        // Auto-fill unit price from MRP when drug is selected
        if (field === 'drugId' && typeof value === 'string') {
          const drug = drugMap.get(value);
          if (drug) updated.unitPrice = drug.mrp ?? 0;
        }
        return updated;
      }),
    );
  };

  // Totals
  const totals = useMemo(() => {
    let subTotal = 0, discAmt = 0, cgst = 0, sgst = 0;
    for (const item of items) {
      const drug = drugMap.get(item.drugId);
      const gst = computeLineGst(item.unitPrice, item.quantity, item.discountPercent, drug?.gstSlab ?? 0);
      subTotal += gst.gross;
      discAmt += gst.discAmt;
      cgst += gst.cgst;
      sgst += gst.sgst;
    }
    return { subTotal, discAmt, cgst, sgst, net: subTotal - discAmt + cgst + sgst };
  }, [items, drugMap]);

  const handleOk = async () => {
    const header = await form.validateFields();

    if (items.length === 0) {
      message.warning('Add at least one line item.');
      return;
    }
    const invalidLine = items.find((i) => !i.drugId || i.quantity <= 0);
    if (invalidLine) {
      message.warning('Each line must have a drug and quantity > 0.');
      return;
    }

    try {
      await createInvoice.mutateAsync({
        invoiceDate: header.invoiceDate.toISOString(),
        paymentMethod: header.paymentMethod,
        status: 'Paid',
        notes: header.notes || null,
        billedBy: currentUser?.username ?? 'system',
        branchId: selectedBranch?.id ?? null,
        pharmacyGstin: selectedBranch?.gstin ?? null,
        items: items.map((i) => ({
          drugId: i.drugId,
          batchNumber: i.batchNumber || null,
          quantity: i.quantity,
          unitPrice: i.unitPrice,
          discountPercent: i.discountPercent,
        })),
      });
      message.success('Invoice created.');
      form.resetFields();
      setItems([]);
      onClose();
    } catch (err: unknown) {
      const msg =
        (err as { response?: { data?: { message?: string } } })?.response?.data?.message ??
        'Failed to create invoice.';
      message.error(msg);
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
      title: 'Batch',
      key: 'batch',
      width: 100,
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
      title: 'Qty',
      key: 'qty',
      width: 70,
      render: (_: unknown, row: LineItem) => (
        <InputNumber
          min={1}
          value={row.quantity}
          onChange={(v) => updateLine(row.key, 'quantity', v ?? 1)}
          style={{ width: '100%' }}
          size="small"
        />
      ),
    },
    {
      title: 'Unit Price (₹)',
      key: 'price',
      width: 110,
      render: (_: unknown, row: LineItem) => {
        const mrp = drugMap.get(row.drugId)?.mrp ?? Infinity;
        return (
          <InputNumber
            min={0}
            max={mrp}
            precision={2}
            value={row.unitPrice}
            onChange={(v) => updateLine(row.key, 'unitPrice', v ?? 0)}
            style={{ width: '100%' }}
            size="small"
          />
        );
      },
    },
    {
      title: 'Disc %',
      key: 'disc',
      width: 75,
      render: (_: unknown, row: LineItem) => (
        <InputNumber
          min={0}
          max={100}
          precision={2}
          value={row.discountPercent}
          onChange={(v) => updateLine(row.key, 'discountPercent', v ?? 0)}
          style={{ width: '100%' }}
          size="small"
        />
      ),
    },
    {
      title: 'GST',
      key: 'gst',
      width: 60,
      align: 'center' as const,
      render: (_: unknown, row: LineItem) => {
        const drug = drugMap.get(row.drugId);
        if (!drug?.gstSlab) return <span style={{ color: '#bbb' }}>—</span>;
        return <Tag color="blue" style={{ fontSize: 11 }}>{drug.gstSlab}%</Tag>;
      },
    },
    {
      title: 'Amount (₹)',
      key: 'amount',
      width: 100,
      align: 'right' as const,
      render: (_: unknown, row: LineItem) => {
        const drug = drugMap.get(row.drugId);
        const { total } = computeLineGst(row.unitPrice, row.quantity, row.discountPercent, drug?.gstSlab ?? 0);
        return <Text>₹{total.toLocaleString('en-IN', { minimumFractionDigits: 2 })}</Text>;
      },
    },
    {
      title: '',
      key: 'del',
      width: 36,
      render: (_: unknown, row: LineItem) => (
        <Button type="text" danger size="small" icon={<DeleteOutlined />} onClick={() => removeLine(row.key)} />
      ),
    },
  ];

  return (
    <Modal
      title="New Invoice"
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); setItems([]); onClose(); }}
      okText="Create Invoice"
      confirmLoading={createInvoice.isPending}
      width={920}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Space direction="horizontal" size={16} style={{ width: '100%', display: 'flex' }}>
          <Form.Item
            name="invoiceDate"
            label="Invoice Date"
            rules={[{ required: true }]}
            style={{ flex: 1, marginBottom: 0 }}
          >
            <DatePicker style={{ width: '100%' }} format="DD MMM YYYY" />
          </Form.Item>
          <Form.Item
            name="paymentMethod"
            label="Payment Mode"
            rules={[{ required: true, message: 'Required' }]}
            style={{ flex: 1, marginBottom: 0 }}
          >
            <Select
              options={PAYMENT_MODES.map((p) => ({
                value: p,
                label: <Tag color={PAYMENT_COLORS[p]}>{p}</Tag>,
              }))}
            />
          </Form.Item>
          <Form.Item name="notes" label="Notes" style={{ flex: 2, marginBottom: 0 }}>
            <Input placeholder="Optional" />
          </Form.Item>
        </Space>
      </Form>

      <Divider orientation="left" style={{ marginTop: 16 }}>Line Items</Divider>

      <Table
        dataSource={items}
        columns={lineColumns}
        rowKey="key"
        pagination={false}
        size="small"
        locale={{ emptyText: 'No items — click Add Item below' }}
        scroll={{ x: 780 }}
      />

      <Space style={{ marginTop: 12, width: '100%', justifyContent: 'space-between', alignItems: 'flex-end' }}>
        <Button icon={<PlusOutlined />} onClick={addLine}>Add Item</Button>
        <Space direction="vertical" size={2} style={{ textAlign: 'right' }}>
          <Text type="secondary" style={{ fontSize: 12 }}>
            Sub Total: ₹{totals.subTotal.toLocaleString('en-IN', { minimumFractionDigits: 2 })}
            {totals.discAmt > 0 && (
              <span style={{ color: '#cf1322', marginLeft: 12 }}>
                − Discount: ₹{totals.discAmt.toLocaleString('en-IN', { minimumFractionDigits: 2 })}
              </span>
            )}
            <span style={{ color: '#1677ff', marginLeft: 12 }}>
              + GST (CGST ₹{totals.cgst.toLocaleString('en-IN', { minimumFractionDigits: 2 })} + SGST ₹{totals.sgst.toLocaleString('en-IN', { minimumFractionDigits: 2 })})
            </span>
          </Text>
          <Text strong style={{ fontSize: 16 }}>
            Net: ₹{totals.net.toLocaleString('en-IN', { minimumFractionDigits: 2 })}
          </Text>
        </Space>
      </Space>
    </Modal>
  );
}
