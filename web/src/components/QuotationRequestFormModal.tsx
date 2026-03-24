import { useState, useEffect, useMemo } from 'react';
import {
  Modal, Form, DatePicker, Input, Table, Button,
  Space, Typography, Divider, InputNumber, message, Select,
} from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import { useDrugs } from '@/hooks/useDrugs';
import { useCreateQuotationRequest } from '@/hooks/useQuotationRequestMutations';
import { useGlobalStore } from '@/store/globalStore';

const { Text } = Typography;

interface LineItem {
  key: string;
  drugId: string;
  quantityRequired: number;
  notes: string;
}

interface Props {
  open: boolean;
  onClose: () => void;
}

type HeaderValues = {
  requiredByDate?: dayjs.Dayjs;
  notes?: string;
};

export default function QuotationRequestFormModal({ open, onClose }: Props) {
  const [form] = Form.useForm<HeaderValues>();
  const [items, setItems] = useState<LineItem[]>([]);

  const currentUser = useGlobalStore((s) => s.currentUser);
  const drugsQuery = useDrugs();
  const createRFQ = useCreateQuotationRequest();

  const drugOptions = useMemo(
    () =>
      (drugsQuery.data ?? [])
        .filter((d) => d.isActive)
        .map((d) => ({
          value: d.id!,
          label: `${d.name ?? d.id}${d.strength ? ` — ${d.strength}` : ''}`,
        })),
    [drugsQuery.data],
  );

  useEffect(() => {
    if (open) { form.resetFields(); setItems([]); }
  }, [open, form]);

  const addLine = () =>
    setItems((prev) => [...prev, { key: crypto.randomUUID(), drugId: '', quantityRequired: 1, notes: '' }]);

  const removeLine = (key: string) => setItems((prev) => prev.filter((i) => i.key !== key));

  const updateLine = (key: string, field: keyof LineItem, value: unknown) =>
    setItems((prev) => prev.map((i) => (i.key === key ? { ...i, [field]: value } : i)));

  const handleOk = async () => {
    const header = await form.validateFields();
    if (items.length === 0) { message.warning('Add at least one drug.'); return; }
    if (items.some((i) => !i.drugId)) { message.warning('All lines must have a drug selected.'); return; }

    try {
      await createRFQ.mutateAsync({
        requestDate: new Date().toISOString(),
        requiredByDate: header.requiredByDate?.toISOString() ?? null,
        status: 'Draft',
        notes: header.notes || null,
        requestedBy: currentUser?.username ?? 'system',
        items: items.map((i) => ({
          drugId: i.drugId,
          quantityRequired: i.quantityRequired,
          notes: i.notes || null,
        })),
      });
      message.success('Quotation request created.');
      onClose();
    } catch {
      message.error('Failed to create quotation request.');
    }
  };

  const columns = [
    {
      title: 'Drug',
      key: 'drug',
      width: 260,
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
      title: 'Qty Required',
      key: 'qty',
      width: 110,
      render: (_: unknown, row: LineItem) => (
        <InputNumber
          min={1}
          value={row.quantityRequired}
          onChange={(v) => updateLine(row.key, 'quantityRequired', v ?? 1)}
          style={{ width: '100%' }}
          size="small"
        />
      ),
    },
    {
      title: 'Notes',
      key: 'notes',
      render: (_: unknown, row: LineItem) => (
        <Input
          value={row.notes}
          onChange={(e) => updateLine(row.key, 'notes', e.target.value)}
          placeholder="Optional"
          size="small"
        />
      ),
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
      title="New Quotation Request (RFQ)"
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); setItems([]); onClose(); }}
      okText="Create RFQ"
      confirmLoading={createRFQ.isPending}
      width={720}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Space direction="horizontal" size={16} style={{ width: '100%', display: 'flex' }}>
          <Form.Item name="requiredByDate" label="Required By" style={{ flex: 1, marginBottom: 0 }}>
            <DatePicker style={{ width: '100%' }} format="DD MMM YYYY" disabledDate={(d) => d.isBefore(dayjs(), 'day')} />
          </Form.Item>
          <Form.Item name="notes" label="Notes" style={{ flex: 2, marginBottom: 0 }}>
            <Input placeholder="Optional notes for vendors" />
          </Form.Item>
        </Space>
      </Form>

      <Divider orientation="left" style={{ marginTop: 16 }}>Drugs Required</Divider>

      <Table
        dataSource={items}
        columns={columns}
        rowKey="key"
        pagination={false}
        size="small"
        locale={{ emptyText: 'No drugs added yet — click Add Drug below' }}
      />

      <Space style={{ marginTop: 12 }}>
        <Button icon={<PlusOutlined />} onClick={addLine}>Add Drug</Button>
        <Text type="secondary">{items.length} item{items.length !== 1 ? 's' : ''}</Text>
      </Space>
    </Modal>
  );
}
