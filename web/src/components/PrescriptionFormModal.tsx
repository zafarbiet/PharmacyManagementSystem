import { useState, useEffect, useMemo } from 'react';
import {
  Modal, Form, DatePicker, Input, InputNumber, Table,
  Button, Space, Typography, Divider, Select, message,
} from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import type { Patient, Prescription, PrescriptionItem } from '@/api/localTypes';
import { useDrugs } from '@/hooks/useDrugs';
import { useGlobalStore } from '@/store/globalStore';

const { Text } = Typography;

interface LineItem {
  key: string;
  drugId: string;
  dosage: string;
  quantity: number;
  instructions: string;
}

interface Props {
  open: boolean;
  patient: Patient;
  onClose: () => void;
  onSubmit: (
    header: Partial<Prescription> & { items: Partial<PrescriptionItem>[] },
  ) => Promise<void>;
  loading: boolean;
}

type HeaderValues = {
  prescribingDoctor?: string;
  doctorRegistrationNumber?: string;
  prescriptionDate: dayjs.Dayjs;
  patientAge?: number;
  notes?: string;
};

export default function PrescriptionFormModal({ open, patient, onClose, onSubmit, loading }: Props) {
  const [form] = Form.useForm<HeaderValues>();
  const [items, setItems] = useState<LineItem[]>([]);

  const currentUser = useGlobalStore((s) => s.currentUser);
  const drugsQuery = useDrugs();

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
    if (open) {
      form.resetFields();
      form.setFieldsValue({ prescriptionDate: dayjs(), patientAge: patient.age ?? undefined });
      setItems([]);
    }
  }, [open, patient, form]);

  const addLine = () =>
    setItems((prev) => [
      ...prev,
      { key: crypto.randomUUID(), drugId: '', dosage: '', quantity: 1, instructions: '' },
    ]);

  const removeLine = (key: string) => setItems((prev) => prev.filter((i) => i.key !== key));

  const updateLine = (key: string, field: keyof LineItem, value: unknown) =>
    setItems((prev) => prev.map((i) => (i.key === key ? { ...i, [field]: value } : i)));

  const handleOk = async () => {
    const header = await form.validateFields();
    if (items.length === 0) { message.warning('Add at least one drug.'); return; }
    if (items.some((i) => !i.drugId)) { message.warning('All lines must have a drug selected.'); return; }

    await onSubmit({
      patientId: patient.id,
      prescribingDoctor: header.prescribingDoctor || null,
      doctorRegistrationNumber: header.doctorRegistrationNumber || null,
      prescriptionDate: header.prescriptionDate.toISOString(),
      patientAge: header.patientAge ?? null,
      notes: header.notes || null,
      updatedBy: currentUser?.username ?? 'system',
      items: items.map((i) => ({
        drugId: i.drugId,
        dosage: i.dosage || null,
        quantity: i.quantity,
        instructions: i.instructions || null,
      })),
    });
  };

  const columns = [
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
      title: 'Dosage',
      key: 'dosage',
      width: 120,
      render: (_: unknown, row: LineItem) => (
        <Input
          value={row.dosage}
          onChange={(e) => updateLine(row.key, 'dosage', e.target.value)}
          placeholder="e.g. 500mg"
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
          value={row.quantity}
          onChange={(v) => updateLine(row.key, 'quantity', v ?? 1)}
          style={{ width: '100%' }}
          size="small"
        />
      ),
    },
    {
      title: 'Instructions',
      key: 'instructions',
      render: (_: unknown, row: LineItem) => (
        <Input
          value={row.instructions}
          onChange={(e) => updateLine(row.key, 'instructions', e.target.value)}
          placeholder="After meals, etc."
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
      title={`New Prescription — ${patient.name ?? 'Patient'}`}
      open={open}
      onOk={handleOk}
      onCancel={onClose}
      okText="Save Prescription"
      confirmLoading={loading}
      width={760}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Space direction="horizontal" size={16} style={{ width: '100%', display: 'flex' }}>
          <Form.Item
            name="prescriptionDate"
            label="Prescription Date"
            rules={[{ required: true, message: 'Required' }]}
            style={{ flex: 1, marginBottom: 0 }}
          >
            <DatePicker style={{ width: '100%' }} format="DD MMM YYYY" />
          </Form.Item>
          <Form.Item name="patientAge" label="Patient Age" style={{ flex: 1, marginBottom: 0 }}>
            <InputNumber min={0} max={150} style={{ width: '100%' }} />
          </Form.Item>
        </Space>

        <Space direction="horizontal" size={16} style={{ width: '100%', display: 'flex', marginTop: 8 }}>
          <Form.Item name="prescribingDoctor" label="Prescribing Doctor" style={{ flex: 2, marginBottom: 0 }}>
            <Input placeholder="Dr. Name" />
          </Form.Item>
          <Form.Item name="doctorRegistrationNumber" label="Reg. Number" style={{ flex: 1, marginBottom: 0 }}>
            <Input placeholder="MCI/State reg." />
          </Form.Item>
        </Space>

        <Form.Item name="notes" label="Notes" style={{ marginTop: 8, marginBottom: 0 }}>
          <Input.TextArea rows={2} placeholder="Additional notes" />
        </Form.Item>
      </Form>

      <Divider orientation="left" style={{ marginTop: 16 }}>Prescribed Drugs</Divider>

      <Table
        dataSource={items}
        columns={columns}
        rowKey="key"
        pagination={false}
        size="small"
        locale={{ emptyText: 'No drugs added — click Add Drug below' }}
      />

      <Space style={{ marginTop: 12 }}>
        <Button icon={<PlusOutlined />} onClick={addLine}>Add Drug</Button>
        <Text type="secondary">{items.length} item{items.length !== 1 ? 's' : ''}</Text>
      </Space>
    </Modal>
  );
}
