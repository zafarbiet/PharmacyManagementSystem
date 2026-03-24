import { useEffect } from 'react';
import { Modal, Form, Select, Input, InputNumber, DatePicker, Row, Col, message } from 'antd';
import dayjs from 'dayjs';
import type { DrugInventory } from '@/api/localTypes';
import { useDrugs } from '@/hooks/useDrugs';
import { useCreateDrugInventory, useUpdateDrugInventory } from '@/hooks/useDrugInventoryMutations';

interface Props {
  open: boolean;
  inventory?: DrugInventory | null;
  onClose: () => void;
}

type FormValues = {
  drugId: string;
  batchNumber?: string;
  expirationDate: dayjs.Dayjs;
  quantityInStock: number;
  storageConditions?: string;
};

export default function DrugInventoryFormModal({ open, inventory, onClose }: Props) {
  const [form] = Form.useForm<FormValues>();
  const isEdit = !!inventory?.id;

  const drugsQuery = useDrugs();
  const drugOptions = (drugsQuery.data ?? [])
    .filter((d) => d.isActive)
    .map((d) => ({ value: d.id!, label: `${d.name ?? d.id}${d.strength ? ` — ${d.strength}` : ''}` }));

  const create = useCreateDrugInventory();
  const update = useUpdateDrugInventory();
  const isPending = create.isPending || update.isPending;

  useEffect(() => {
    if (open) {
      form.setFieldsValue(
        inventory
          ? {
              drugId: inventory.drugId,
              batchNumber: inventory.batchNumber ?? '',
              expirationDate: dayjs(inventory.expirationDate),
              quantityInStock: inventory.quantityInStock,
              storageConditions: inventory.storageConditions ?? '',
            }
          : { quantityInStock: 0 },
      );
    }
  }, [open, inventory, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    const payload = {
      ...values,
      expirationDate: values.expirationDate.toISOString(),
    };
    try {
      if (isEdit && inventory?.id) {
        await update.mutateAsync({ id: inventory.id, ...payload });
        message.success('Inventory entry updated.');
      } else {
        await create.mutateAsync(payload);
        message.success('Inventory entry added.');
      }
      form.resetFields();
      onClose();
    } catch {
      message.error('Failed to save. Please try again.');
    }
  };

  const handleCancel = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      title={isEdit ? 'Edit Inventory Entry' : 'Add Inventory Entry'}
      open={open}
      onOk={handleOk}
      onCancel={handleCancel}
      okText={isEdit ? 'Save Changes' : 'Add Entry'}
      confirmLoading={isPending}
      width={580}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Form.Item
          name="drugId"
          label="Drug"
          rules={[{ required: true, message: 'Please select a drug' }]}
        >
          <Select
            showSearch
            placeholder="Search drug…"
            options={drugOptions}
            loading={drugsQuery.isLoading}
            filterOption={(input, opt) =>
              (opt?.label ?? '').toLowerCase().includes(input.toLowerCase())
            }
            disabled={isEdit}
          />
        </Form.Item>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="batchNumber" label="Batch Number">
              <Input placeholder="e.g. BT-2024-001" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              name="expirationDate"
              label="Expiration Date"
              rules={[{ required: true, message: 'Expiration date is required' }]}
            >
              <DatePicker
                style={{ width: '100%' }}
                format="DD MMM YYYY"
                disabledDate={(d) => d.isBefore(dayjs(), 'day')}
              />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item
              name="quantityInStock"
              label="Quantity in Stock"
              rules={[{ required: true, message: 'Quantity is required' }]}
            >
              <InputNumber min={0} style={{ width: '100%' }} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="storageConditions" label="Storage Conditions">
              <Input placeholder="e.g. Store below 25°C" />
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Modal>
  );
}
