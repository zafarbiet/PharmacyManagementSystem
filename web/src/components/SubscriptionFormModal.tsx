import { useEffect, useState } from 'react';
import {
  Modal, Form, Select, InputNumber, DatePicker, Input,
  Table, Button, Space, Popconfirm, message,
} from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import type { CustomerSubscription, CustomerSubscriptionItem } from '@/hooks/useSubscriptions';
import { useSubscriptionItems } from '@/hooks/useSubscriptions';
import {
  useCreateSubscription, useUpdateSubscription,
  useCreateSubscriptionItem, useDeleteSubscriptionItem,
} from '@/hooks/useSubscriptionMutations';
import { usePatients } from '@/hooks/usePatients';
import { useDrugs } from '@/hooks/useDrugs';

interface Props {
  open: boolean;
  subscription: CustomerSubscription | null;
  onClose: () => void;
}

type FormValues = {
  patientId: string;
  startDate: dayjs.Dayjs;
  endDate?: dayjs.Dayjs;
  cycleDayOfMonth: number;
  status: string;
  notes?: string;
};

const STATUS_OPTIONS = ['Active', 'Paused', 'Cancelled', 'Expired'].map((s) => ({ value: s, label: s }));

export default function SubscriptionFormModal({ open, subscription, onClose }: Props) {
  const [form] = Form.useForm<FormValues>();
  const isEdit = !!subscription?.id;

  const patientsQuery = usePatients();
  const drugsQuery = useDrugs();
  const itemsQuery = useSubscriptionItems(subscription?.id);

  const createSub = useCreateSubscription();
  const updateSub = useUpdateSubscription();
  const createItem = useCreateSubscriptionItem();
  const deleteItem = useDeleteSubscriptionItem();

  const [newDrugId, setNewDrugId] = useState<string | undefined>();
  const [newQty, setNewQty] = useState<number>(1);

  const patientOptions = (patientsQuery.data ?? []).map((p) => ({
    value: p.id, label: p.name ?? p.id,
  }));
  const drugOptions = (drugsQuery.data ?? [])
    .filter((d) => d.isActive)
    .map((d) => ({ value: d.id!, label: `${d.name ?? d.id}${d.strength ? ` — ${d.strength}` : ''}` }));
  const drugMap = new Map((drugsQuery.data ?? []).map((d) => [d.id ?? '', d.name ?? d.id ?? '']));

  useEffect(() => {
    if (open) {
      form.setFieldsValue(
        subscription
          ? {
              patientId: subscription.patientId,
              startDate: dayjs(subscription.startDate),
              endDate: subscription.endDate ? dayjs(subscription.endDate) : undefined,
              cycleDayOfMonth: subscription.cycleDayOfMonth,
              status: subscription.status ?? 'Active',
              notes: subscription.notes ?? '',
            }
          : { cycleDayOfMonth: 1, status: 'Active' },
      );
      setNewDrugId(undefined);
      setNewQty(1);
    }
  }, [open, subscription, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    const payload = {
      ...values,
      startDate: values.startDate.toISOString(),
      endDate: values.endDate?.toISOString() ?? null,
      approvalStatus: isEdit ? subscription?.approvalStatus : 'Pending',
    };
    try {
      if (isEdit && subscription?.id) {
        await updateSub.mutateAsync({ id: subscription.id, ...payload });
        message.success('Subscription updated.');
      } else {
        await createSub.mutateAsync(payload);
        message.success('Subscription created. Pending approval.');
      }
      form.resetFields();
      onClose();
    } catch {
      message.error('Failed to save subscription.');
    }
  };

  const handleAddItem = async () => {
    if (!newDrugId || !subscription?.id) return;
    try {
      await createItem.mutateAsync({
        subscriptionId: subscription.id,
        drugId: newDrugId,
        quantityPerCycle: newQty,
      });
      setNewDrugId(undefined);
      setNewQty(1);
      message.success('Drug added.');
    } catch {
      message.error('Failed to add drug.');
    }
  };

  const handleDeleteItem = async (id: string) => {
    try { await deleteItem.mutateAsync(id); message.success('Drug removed.'); }
    catch { message.error('Failed to remove drug.'); }
  };

  const items: CustomerSubscriptionItem[] = itemsQuery.data ?? [];

  return (
    <Modal
      title={isEdit ? 'Edit Subscription' : 'New Subscription'}
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); onClose(); }}
      okText={isEdit ? 'Save Changes' : 'Create'}
      confirmLoading={createSub.isPending || updateSub.isPending}
      width={640}
      destroyOnClose
    >
      <Form form={form} layout="vertical" style={{ marginTop: 16 }}>
        <Form.Item name="patientId" label="Patient" rules={[{ required: true }]}>
          <Select showSearch options={patientOptions} placeholder="Select patient"
            filterOption={(i, o) => (o?.label ?? '').toLowerCase().includes(i.toLowerCase())}
            disabled={isEdit} />
        </Form.Item>
        <Form.Item name="startDate" label="Start Date" rules={[{ required: true }]}>
          <DatePicker style={{ width: '100%' }} format="DD MMM YYYY" />
        </Form.Item>
        <Form.Item name="endDate" label="End Date (leave blank for ongoing)">
          <DatePicker style={{ width: '100%' }} format="DD MMM YYYY" />
        </Form.Item>
        <Form.Item name="cycleDayOfMonth" label="Cycle Day of Month" rules={[{ required: true }]}>
          <InputNumber min={1} max={28} style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="status" label="Status" rules={[{ required: true }]}>
          <Select options={STATUS_OPTIONS} />
        </Form.Item>
        <Form.Item name="notes" label="Notes">
          <Input.TextArea rows={2} />
        </Form.Item>
      </Form>

      {isEdit && (
        <>
          <div style={{ fontWeight: 600, marginBottom: 8 }}>Drugs per Cycle</div>
          <Table
            size="small"
            rowKey="id"
            pagination={false}
            dataSource={items}
            loading={itemsQuery.isLoading}
            columns={[
              { title: 'Drug', dataIndex: 'drugId', render: (id: string) => drugMap.get(id) ?? id },
              { title: 'Qty', dataIndex: 'quantityPerCycle', align: 'right', width: 60 },
              {
                title: '',
                key: 'del',
                width: 40,
                render: (_: unknown, row: CustomerSubscriptionItem) => (
                  <Popconfirm title="Remove this drug?" onConfirm={() => handleDeleteItem(row.id)} okText="Remove">
                    <Button type="text" size="small" danger icon={<DeleteOutlined />} />
                  </Popconfirm>
                ),
              },
            ]}
          />
          <Space style={{ marginTop: 8 }}>
            <Select
              showSearch
              placeholder="Add drug…"
              options={drugOptions}
              value={newDrugId}
              onChange={setNewDrugId}
              style={{ width: 260 }}
              filterOption={(i, o) => (o?.label ?? '').toLowerCase().includes(i.toLowerCase())}
            />
            <InputNumber min={1} value={newQty} onChange={(v) => setNewQty(v ?? 1)} style={{ width: 70 }} />
            <Button icon={<PlusOutlined />} onClick={handleAddItem} loading={createItem.isPending}>Add</Button>
          </Space>
        </>
      )}
    </Modal>
  );
}
