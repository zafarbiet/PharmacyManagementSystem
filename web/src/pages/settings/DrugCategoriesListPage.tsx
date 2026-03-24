import { useState } from 'react';
import { Table, Button, Tag, Space, Typography, Row, Col, Card, Popconfirm, message, Input } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, ReloadOutlined, SearchOutlined, TagsOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { DrugCategory } from '@/api/types.gen';
import { useDrugCategories } from '@/hooks/useDrugCategories';
import { useDeleteDrugCategory } from '@/hooks/useDrugCategoryMutations';
import DrugCategoryFormModal from '@/components/DrugCategoryFormModal';

const { Title } = Typography;

export default function DrugCategoriesListPage() {
  const [search, setSearch] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<DrugCategory | null>(null);

  const categoriesQuery = useDrugCategories();
  const deleteCategory = useDeleteDrugCategory();

  const categories = categoriesQuery.data ?? [];

  const filtered = search
    ? categories.filter(
        (c) =>
          (c.name ?? '').toLowerCase().includes(search.toLowerCase()) ||
          (c.description ?? '').toLowerCase().includes(search.toLowerCase()),
      )
    : categories;

  const handleDelete = async (id: string) => {
    try {
      await deleteCategory.mutateAsync(id);
      message.success('Category deleted.');
    } catch {
      message.error('Failed to delete category. It may be in use by drugs.');
    }
  };

  const columns: ColumnsType<DrugCategory> = [
    {
      title: 'Category Name',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => (a.name ?? '').localeCompare(b.name ?? ''),
      render: (v: string | null) => <strong>{v ?? '—'}</strong>,
    },
    {
      title: 'Description',
      dataIndex: 'description',
      key: 'description',
      ellipsis: true,
      render: (v: string | null) => v ?? <span style={{ color: '#bbb' }}>—</span>,
    },
    {
      title: 'Status',
      dataIndex: 'isActive',
      key: 'isActive',
      render: (v: boolean) => <Tag color={v ? 'green' : 'default'}>{v ? 'Active' : 'Inactive'}</Tag>,
    },
    {
      title: 'Last Updated',
      dataIndex: 'updatedAt',
      key: 'updatedAt',
      render: (v: string) => (v ? dayjs(v).format('DD MMM YYYY') : '—'),
      sorter: (a, b) => dayjs(a.updatedAt).unix() - dayjs(b.updatedAt).unix(),
      defaultSortOrder: 'descend',
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 90,
      render: (_: unknown, record: DrugCategory) => (
        <Space size={4}>
          <Button
            type="text"
            size="small"
            icon={<EditOutlined />}
            onClick={() => { setEditingCategory(record); setModalOpen(true); }}
          />
          <Popconfirm
            title="Delete this category?"
            description="Drugs using this category will be affected."
            onConfirm={() => handleDelete(record.id!)}
            okText="Delete"
            okType="danger"
          >
            <Button type="text" size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            <TagsOutlined style={{ marginRight: 8 }} />
            Drug Categories
          </Title>
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => { setEditingCategory(null); setModalOpen(true); }}
          >
            Add Category
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Categories</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{categories.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={12}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Active</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#3f8600' }}>
              {categories.filter((c) => c.isActive).length}
            </div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <Input
            placeholder="Search by name or description…"
            prefix={<SearchOutlined />}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            allowClear
            onClear={() => setSearch('')}
          />
        </Col>
        <Col>
          <Button icon={<ReloadOutlined />} onClick={() => { setSearch(''); categoriesQuery.refetch(); }} />
        </Col>
      </Row>

      <Table<DrugCategory>
        rowKey="id"
        columns={columns}
        dataSource={filtered}
        loading={categoriesQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} categories` }}
        scroll={{ x: 600 }}
      />

      <DrugCategoryFormModal
        open={modalOpen}
        category={editingCategory}
        onClose={() => setModalOpen(false)}
      />
    </div>
  );
}
