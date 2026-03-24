import { useEffect } from 'react';
import { Modal, Form, Input, message } from 'antd';
import type { DrugCategory } from '@/api/types.gen';
import { useCreateDrugCategory, useUpdateDrugCategory } from '@/hooks/useDrugCategoryMutations';

interface Props {
  open: boolean;
  category?: DrugCategory | null;
  onClose: () => void;
}

type FormValues = {
  name: string;
  description?: string;
};

export default function DrugCategoryFormModal({ open, category, onClose }: Props) {
  const [form] = Form.useForm<FormValues>();
  const isEdit = !!category?.id;

  const create = useCreateDrugCategory();
  const update = useUpdateDrugCategory();
  const isPending = create.isPending || update.isPending;

  useEffect(() => {
    if (open) {
      form.setFieldsValue(
        category
          ? { name: category.name ?? '', description: category.description ?? '' }
          : { name: '', description: '' },
      );
    }
  }, [open, category, form]);

  const handleOk = async () => {
    const values = await form.validateFields();
    try {
      if (isEdit && category?.id) {
        await update.mutateAsync({ id: category.id, ...values });
        message.success('Category updated.');
      } else {
        await create.mutateAsync(values);
        message.success('Category added.');
      }
      form.resetFields();
      onClose();
    } catch {
      message.error('Failed to save category.');
    }
  };

  return (
    <Modal
      title={isEdit ? 'Edit Category' : 'Add Category'}
      open={open}
      onOk={handleOk}
      onCancel={() => { form.resetFields(); onClose(); }}
      okText={isEdit ? 'Save Changes' : 'Add Category'}
      confirmLoading={isPending}
      width={440}
      destroyOnClose
    >
      <Form form={form} layout="vertical" size="middle" style={{ marginTop: 16 }}>
        <Form.Item name="name" label="Category Name" rules={[{ required: true, message: 'Required' }]}>
          <Input placeholder="e.g. Antibiotics" />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={3} placeholder="Optional description" />
        </Form.Item>
      </Form>
    </Modal>
  );
}
