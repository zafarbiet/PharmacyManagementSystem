import { useEffect } from 'react';
import { Modal, Form, Input, InputNumber, Row, Col, message } from 'antd';
import type { Vendor } from '@/api/types.gen';
import { useCreateVendor, useUpdateVendor } from '@/hooks/useVendorMutations';

interface Props {
  open: boolean;
  vendor?: Vendor | null;
  onClose: () => void;
}

type FormValues = {
  name: string;
  contactPerson?: string;
  phone?: string;
  email?: string;
  address?: string;
  gstNumber?: string;
  drugLicenseNumber?: string;
  creditTermsDays: number;
  creditLimit: number;
};

export default function VendorFormModal({ open, vendor, onClose }: Props) {
  const [form] = Form.useForm<FormValues>();
  const isEdit = !!vendor?.id;

  const create = useCreateVendor();
  const update = useUpdateVendor();
  const isPending = create.isPending || update.isPending;

  useEffect(() => {
    if (open) {
      form.setFieldsValue(
        vendor
          ? {
              name: vendor.name ?? '',
              contactPerson: vendor.contactPerson ?? '',
              phone: vendor.phone ?? '',
              email: vendor.email ?? '',
              address: vendor.address ?? '',
              gstNumber: vendor.gstNumber ?? '',
              drugLicenseNumber: vendor.drugLicenseNumber ?? '',
              creditTermsDays: vendor.creditTermsDays ?? 30,
              creditLimit: vendor.creditLimit ?? 0,
            }
          : { creditTermsDays: 30, creditLimit: 0 },
      );
    }
  }, [open, vendor, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    try {
      if (isEdit && vendor?.id) {
        await update.mutateAsync({ id: vendor.id, ...values });
        message.success('Vendor updated successfully.');
      } else {
        await create.mutateAsync(values);
        message.success('Vendor added successfully.');
      }
      form.resetFields();
      onClose();
    } catch {
      message.error('Failed to save vendor. Please try again.');
    }
  };

  const handleCancel = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      title={isEdit ? 'Edit Vendor' : 'Add Vendor'}
      open={open}
      onOk={handleOk}
      onCancel={handleCancel}
      okText={isEdit ? 'Save Changes' : 'Add Vendor'}
      confirmLoading={isPending}
      width={640}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Form.Item name="name" label="Vendor Name" rules={[{ required: true, message: 'Name is required' }]}>
          <Input placeholder="e.g. Sun Pharma Distributors" />
        </Form.Item>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="contactPerson" label="Contact Person">
              <Input placeholder="e.g. Rajesh Kumar" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="phone" label="Phone">
              <Input placeholder="e.g. 9876543210" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item
              name="email"
              label="Email"
              rules={[{ type: 'email', message: 'Enter a valid email' }]}
            >
              <Input placeholder="e.g. orders@sunpharma.com" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="gstNumber" label="GST Number">
              <Input placeholder="e.g. 27AAPFU0939F1ZV" maxLength={15} />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item name="address" label="Address">
          <Input.TextArea rows={2} placeholder="Full address" />
        </Form.Item>

        <Form.Item name="drugLicenseNumber" label="Drug License Number">
          <Input placeholder="e.g. MH-MUM-123456" />
        </Form.Item>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item
              name="creditTermsDays"
              label="Credit Terms (days)"
              rules={[{ required: true, message: 'Required' }]}
            >
              <InputNumber min={0} max={365} style={{ width: '100%' }} addonAfter="days" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              name="creditLimit"
              label="Credit Limit (₹)"
              rules={[{ required: true, message: 'Required' }]}
            >
              <InputNumber min={0} style={{ width: '100%' }} prefix="₹" formatter={(v) => `${v}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')} />
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Modal>
  );
}
