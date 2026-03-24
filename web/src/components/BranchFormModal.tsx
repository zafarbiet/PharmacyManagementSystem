import { useEffect } from 'react';
import { Modal, Form, Input, Row, Col, message } from 'antd';
import type { Branch } from '@/api/localTypes';
import { useCreateBranch, useUpdateBranch } from '@/hooks/useBranchMutations';

interface Props {
  open: boolean;
  branch?: Branch | null;
  onClose: () => void;
}

type FormValues = {
  name: string;
  address?: string;
  gstin?: string;
  pharmacyLicenseNumber?: string;
  phone?: string;
  email?: string;
};

export default function BranchFormModal({ open, branch, onClose }: Props) {
  const [form] = Form.useForm<FormValues>();
  const isEdit = !!branch?.id;

  const create = useCreateBranch();
  const update = useUpdateBranch();
  const isPending = create.isPending || update.isPending;

  useEffect(() => {
    if (open) {
      form.setFieldsValue(
        branch
          ? {
              name: branch.name ?? '',
              address: branch.address ?? '',
              gstin: branch.gstin ?? '',
              pharmacyLicenseNumber: branch.pharmacyLicenseNumber ?? '',
              phone: branch.phone ?? '',
              email: branch.email ?? '',
            }
          : { name: '', address: '', gstin: '', pharmacyLicenseNumber: '', phone: '', email: '' },
      );
    }
  }, [open, branch, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    try {
      if (isEdit && branch?.id) {
        await update.mutateAsync({ id: branch.id, ...values });
        message.success('Branch updated.');
      } else {
        await create.mutateAsync(values);
        message.success('Branch added.');
      }
      form.resetFields();
      onClose();
    } catch {
      message.error('Failed to save branch.');
    }
  };

  return (
    <Modal
      title={isEdit ? 'Edit Branch' : 'Add Branch'}
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); onClose(); }}
      okText={isEdit ? 'Save Changes' : 'Add Branch'}
      confirmLoading={isPending}
      width={600}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Form.Item name="name" label="Branch Name" rules={[{ required: true, message: 'Required' }]}>
          <Input placeholder="e.g. Main Branch" />
        </Form.Item>

        <Form.Item name="address" label="Address">
          <Input.TextArea rows={2} placeholder="Full address" />
        </Form.Item>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="gstin" label="GSTIN">
              <Input placeholder="e.g. 27AABCU9603R1ZX" maxLength={15} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="pharmacyLicenseNumber" label="Pharmacy License No.">
              <Input placeholder="e.g. MH-MUM-123456" />
            </Form.Item>
          </Col>
        </Row>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="phone" label="Phone">
              <Input placeholder="e.g. 9876543210" maxLength={15} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="email" label="Email">
              <Input type="email" placeholder="e.g. branch@pharmacy.in" />
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Modal>
  );
}
