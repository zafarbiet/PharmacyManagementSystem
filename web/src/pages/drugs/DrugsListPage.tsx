import { useState, useMemo } from 'react';
import { Table, Input, Select, Button, Tag, Space, Typography, Row, Col, Card, Popconfirm, message } from 'antd';
import { SearchOutlined, PlusOutlined, ReloadOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import type { Drug } from '@/api/types.gen';
import { useDrugs } from '@/hooks/useDrugs';
import { useDrugCategories } from '@/hooks/useDrugCategories';
import { useDeleteDrug } from '@/hooks/useDrugMutations';
import DrugFormModal from '@/components/DrugFormModal';

const { Title } = Typography;

export default function DrugsListPage() {
  const [search, setSearch] = useState('');
  const [appliedSearch, setAppliedSearch] = useState('');
  const [selectedCategory, setSelectedCategory] = useState<string | undefined>();
  const [modalOpen, setModalOpen] = useState(false);
  const [editingDrug, setEditingDrug] = useState<Drug | null>(null);

  const drugsQuery = useDrugs();
  const categoriesQuery = useDrugCategories();
  const deleteDrug = useDeleteDrug();

  const categoryMap = useMemo(() => {
    const map = new Map<string, string>();
    for (const c of categoriesQuery.data ?? []) {
      if (c.id) map.set(c.id, c.name ?? c.id);
    }
    return map;
  }, [categoriesQuery.data]);

  const filtered = useMemo(() => {
    let rows = drugsQuery.data ?? [];
    if (appliedSearch) {
      const lower = appliedSearch.toLowerCase();
      rows = rows.filter(
        (d) =>
          (d.name ?? '').toLowerCase().includes(lower) ||
          (d.genericName ?? '').toLowerCase().includes(lower) ||
          (d.brandName ?? '').toLowerCase().includes(lower) ||
          (d.composition ?? '').toLowerCase().includes(lower),
      );
    }
    if (selectedCategory) {
      rows = rows.filter((d) => d.categoryId === selectedCategory);
    }
    return rows;
  }, [drugsQuery.data, appliedSearch, selectedCategory]);

  const categoryOptions = useMemo(
    () =>
      (categoriesQuery.data ?? []).map((c) => ({
        value: c.id,
        label: c.name ?? c.id,
      })),
    [categoriesQuery.data],
  );

  const columns: ColumnsType<Drug> = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => (a.name ?? '').localeCompare(b.name ?? ''),
      render: (v: string | null) => <strong>{v ?? '—'}</strong>,
    },
    {
      title: 'Generic Name',
      dataIndex: 'genericName',
      key: 'genericName',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Brand',
      dataIndex: 'brandName',
      key: 'brandName',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Category',
      dataIndex: 'categoryId',
      key: 'categoryId',
      render: (id: string) => categoryMap.get(id) ?? id,
    },
    {
      title: 'Dosage Form',
      dataIndex: 'dosageForm',
      key: 'dosageForm',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Strength',
      dataIndex: 'strength',
      key: 'strength',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'MRP',
      dataIndex: 'mrp',
      key: 'mrp',
      align: 'right',
      sorter: (a, b) => (a.mrp ?? 0) - (b.mrp ?? 0),
      render: (v: number) =>
        (v ?? 0).toLocaleString('en-IN', { style: 'currency', currency: 'INR' }),
    },
    {
      title: 'GST %',
      dataIndex: 'gstSlab',
      key: 'gstSlab',
      align: 'right',
      render: (v: number) => `${v ?? 0}%`,
      sorter: (a, b) => (a.gstSlab ?? 0) - (b.gstSlab ?? 0),
    },
    {
      title: 'Schedule',
      dataIndex: 'scheduleCategory',
      key: 'scheduleCategory',
      render: (v: string | null) =>
        v ? <Tag color="orange">{v}</Tag> : <Tag color="default">OTC</Tag>,
    },
    {
      title: 'Rx',
      dataIndex: 'prescriptionRequired',
      key: 'prescriptionRequired',
      render: (v: boolean) => (v ? <Tag color="red">Rx</Tag> : <Tag color="green">OTC</Tag>),
    },
    {
      title: 'Status',
      dataIndex: 'isActive',
      key: 'isActive',
      render: (v: boolean) => <Tag color={v ? 'green' : 'default'}>{v ? 'Active' : 'Inactive'}</Tag>,
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 90,
      render: (_: unknown, record: Drug) => (
        <Space size={4}>
          <Button type="text" size="small" icon={<EditOutlined />} onClick={() => { setEditingDrug(record); setModalOpen(true); }} />
          <Popconfirm
            title="Delete this drug?"
            description="This will soft-delete the drug record."
            onConfirm={() => handleDelete(record.id!)}
            okText="Delete"
            okType="danger"
          >
            <Button type="text" size="small" danger icon={<DeleteOutlined />} loading={deleteDrug.isPending} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  const handleDelete = async (id: string) => {
    try {
      await deleteDrug.mutateAsync(id);
      message.success('Drug deleted.');
    } catch {
      message.error('Failed to delete drug.');
    }
  };

  const rxCount = filtered.filter((d) => d.prescriptionRequired).length;

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            Drug Master
          </Title>
        </Col>
        <Col>
          <Button type="primary" icon={<PlusOutlined />} onClick={() => { setEditingDrug(null); setModalOpen(true); }}>
            Add Drug
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Drugs</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{filtered.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Prescription Required</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#cf1322' }}>{rxCount}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Categories</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{categoryMap.size}</div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <Input
            placeholder="Search by name, generic, brand or composition…"
            prefix={<SearchOutlined />}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onPressEnter={() => setAppliedSearch(search)}
            allowClear
            onClear={() => {
              setSearch('');
              setAppliedSearch('');
            }}
          />
        </Col>
        <Col style={{ width: 200 }}>
          <Select
            placeholder="All Categories"
            options={categoryOptions}
            value={selectedCategory}
            onChange={setSelectedCategory}
            allowClear
            style={{ width: '100%' }}
          />
        </Col>
        <Col>
          <Space>
            <Button type="primary" icon={<SearchOutlined />} onClick={() => setAppliedSearch(search)}>
              Search
            </Button>
            <Button
              icon={<ReloadOutlined />}
              onClick={() => {
                setSearch('');
                setAppliedSearch('');
                setSelectedCategory(undefined);
                drugsQuery.refetch();
              }}
            />
          </Space>
        </Col>
      </Row>

      <Table<Drug>
        rowKey="id"
        columns={columns}
        dataSource={filtered}
        loading={drugsQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} drugs` }}
        scroll={{ x: 1100 }}
      />

      <DrugFormModal
        open={modalOpen}
        drug={editingDrug}
        onClose={() => setModalOpen(false)}
      />
    </div>
  );
}
