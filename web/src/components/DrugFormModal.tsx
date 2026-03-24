import { useEffect } from 'react';
import { Modal, Form, Input, InputNumber, Select, Switch, Row, Col, message } from 'antd';
import type { Drug } from '@/api/types.gen';
import { useDrugCategories } from '@/hooks/useDrugCategories';
import { useCreateDrug, useUpdateDrug } from '@/hooks/useDrugMutations';

interface Props {
  open: boolean;
  drug?: Drug | null;
  onClose: () => void;
}

const DOSAGE_FORMS = ['Tablet', 'Capsule', 'Syrup', 'Injection', 'Cream', 'Ointment', 'Drops', 'Inhaler', 'Patch', 'Suppository', 'Powder', 'Suspension'];
const GST_SLABS = [0, 5, 12, 18, 28];
const SCHEDULE_CATS = ['H', 'H1', 'X', 'G', 'OTC'];

type FormValues = {
  name: string;
  categoryId: string;
  genericName?: string;
  brandName?: string;
  manufacturerName?: string;
  dosageForm?: string;
  strength?: string;
  unitOfMeasure?: string;
  composition?: string;
  scheduleCategory?: string;
  prescriptionRequired: boolean;
  hsnCode?: string;
  gstSlab: number;
  mrp: number;
  reorderLevel: number;
  description?: string;
};

export default function DrugFormModal({ open, drug, onClose }: Props) {
  const [form] = Form.useForm<FormValues>();
  const isEdit = !!drug?.id;

  const categoriesQuery = useDrugCategories();
  const categoryOptions = (categoriesQuery.data ?? []).map((c) => ({
    value: c.id!,
    label: c.name ?? c.id,
  }));

  const create = useCreateDrug();
  const update = useUpdateDrug();
  const isPending = create.isPending || update.isPending;

  useEffect(() => {
    if (open) {
      form.setFieldsValue(
        drug
          ? {
              name: drug.name ?? '',
              categoryId: drug.categoryId!,
              genericName: drug.genericName ?? '',
              brandName: drug.brandName ?? '',
              manufacturerName: drug.manufacturerName ?? '',
              dosageForm: drug.dosageForm ?? undefined,
              strength: drug.strength ?? '',
              unitOfMeasure: drug.unitOfMeasure ?? '',
              composition: drug.composition ?? '',
              scheduleCategory: drug.scheduleCategory ?? undefined,
              prescriptionRequired: drug.prescriptionRequired ?? false,
              hsnCode: drug.hsnCode ?? '',
              gstSlab: drug.gstSlab ?? 12,
              mrp: drug.mrp ?? 0,
              reorderLevel: drug.reorderLevel ?? 0,
              description: drug.description ?? '',
            }
          : { gstSlab: 12, mrp: 0, reorderLevel: 10, prescriptionRequired: false },
      );
    }
  }, [open, drug, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    try {
      if (isEdit && drug?.id) {
        await update.mutateAsync({ id: drug.id, ...values });
        message.success('Drug updated successfully.');
      } else {
        await create.mutateAsync(values);
        message.success('Drug added successfully.');
      }
      form.resetFields();
      onClose();
    } catch {
      message.error('Failed to save drug. Please try again.');
    }
  };

  return (
    <Modal
      title={isEdit ? 'Edit Drug' : 'Add Drug'}
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); onClose(); }}
      okText={isEdit ? 'Save Changes' : 'Add Drug'}
      confirmLoading={isPending}
      width={720}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="name" label="Drug Name" rules={[{ required: true, message: 'Required' }]}>
              <Input placeholder="e.g. Paracetamol" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="categoryId" label="Category" rules={[{ required: true, message: 'Required' }]}>
              <Select
                showSearch
                placeholder="Select category"
                options={categoryOptions}
                loading={categoriesQuery.isLoading}
                filterOption={(i, o) => (o?.label ?? '').toLowerCase().includes(i.toLowerCase())}
              />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="genericName" label="Generic Name">
              <Input placeholder="e.g. Acetaminophen" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="brandName" label="Brand Name">
              <Input placeholder="e.g. Crocin" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="manufacturerName" label="Manufacturer">
              <Input placeholder="e.g. GSK Pharma" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="dosageForm" label="Dosage Form">
              <Select
                placeholder="Select form"
                options={DOSAGE_FORMS.map((f) => ({ value: f, label: f }))}
                allowClear
              />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={8}>
            <Form.Item name="strength" label="Strength">
              <Input placeholder="e.g. 500mg" />
            </Form.Item>
          </Col>
          <Col span={8}>
            <Form.Item name="unitOfMeasure" label="Unit of Measure">
              <Input placeholder="e.g. Tablet, ml" />
            </Form.Item>
          </Col>
          <Col span={8}>
            <Form.Item name="scheduleCategory" label="Schedule">
              <Select
                placeholder="OTC / H / X…"
                options={SCHEDULE_CATS.map((s) => ({ value: s, label: s }))}
                allowClear
              />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item name="composition" label="Composition">
          <Input.TextArea rows={2} placeholder="e.g. Paracetamol 500mg + Caffeine 65mg" />
        </Form.Item>

        <Row gutter={16}>
          <Col span={6}>
            <Form.Item name="mrp" label="MRP (₹)" rules={[{ required: true, message: 'Required' }]}>
              <InputNumber min={0} style={{ width: '100%' }} prefix="₹" precision={2} />
            </Form.Item>
          </Col>
          <Col span={6}>
            <Form.Item name="gstSlab" label="GST %" rules={[{ required: true, message: 'Required' }]}>
              <Select options={GST_SLABS.map((g) => ({ value: g, label: `${g}%` }))} />
            </Form.Item>
          </Col>
          <Col span={6}>
            <Form.Item name="hsnCode" label="HSN Code">
              <Input placeholder="e.g. 3004" maxLength={8} />
            </Form.Item>
          </Col>
          <Col span={6}>
            <Form.Item name="reorderLevel" label="Reorder Level">
              <InputNumber min={0} style={{ width: '100%' }} />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={6}>
            <Form.Item name="prescriptionRequired" label="Prescription Required" valuePropName="checked">
              <Switch checkedChildren="Rx" unCheckedChildren="OTC" />
            </Form.Item>
          </Col>
          <Col span={18}>
            <Form.Item name="description" label="Description">
              <Input.TextArea rows={2} placeholder="Optional notes" />
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Modal>
  );
}
