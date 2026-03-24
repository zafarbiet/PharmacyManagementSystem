import { useState } from 'react';
import { Table, Button, Tag, Space, Typography, Row, Col, Card, Popconfirm, message, Input } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, ReloadOutlined, SearchOutlined, BankOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import type { Branch } from '@/api/localTypes';
import { useBranches } from '@/hooks/useBranches';
import { useDeleteBranch } from '@/hooks/useBranchMutations';
import BranchFormModal from '@/components/BranchFormModal';
import { useGlobalStore } from '@/store/globalStore';

const { Title } = Typography;

export default function BranchesListPage() {
  const [search, setSearch] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingBranch, setEditingBranch] = useState<Branch | null>(null);

  const branchesQuery = useBranches();
  const deleteBranch = useDeleteBranch();
  const selectedBranch = useGlobalStore((s) => s.selectedBranch);
  const setSelectedBranch = useGlobalStore((s) => s.setSelectedBranch);

  const branches = (branchesQuery.data ?? []) as Branch[];

  const filtered = search
    ? branches.filter(
        (b) =>
          (b.name ?? '').toLowerCase().includes(search.toLowerCase()) ||
          (b.gstin ?? '').toLowerCase().includes(search.toLowerCase()) ||
          (b.pharmacyLicenseNumber ?? '').toLowerCase().includes(search.toLowerCase()),
      )
    : branches;

  const handleDelete = async (id: string) => {
    try {
      await deleteBranch.mutateAsync(id);
      message.success('Branch deleted.');
    } catch {
      message.error('Failed to delete branch.');
    }
  };

  const columns: ColumnsType<Branch> = [
    {
      title: 'Branch Name',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => (a.name ?? '').localeCompare(b.name ?? ''),
      render: (v: string | null, record: Branch) => (
        <Space>
          <strong>{v ?? '—'}</strong>
          {record.id === (selectedBranch as unknown as Branch)?.id && (
            <Tag color="blue" style={{ fontSize: 11 }}>Selected</Tag>
          )}
        </Space>
      ),
    },
    {
      title: 'GSTIN',
      dataIndex: 'gstin',
      key: 'gstin',
      render: (v: string | null) =>
        v ? <Tag color="purple" style={{ fontFamily: 'monospace' }}>{v}</Tag> : <span style={{ color: '#bbb' }}>—</span>,
    },
    {
      title: 'License No.',
      dataIndex: 'pharmacyLicenseNumber',
      key: 'pharmacyLicenseNumber',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Phone',
      dataIndex: 'phone',
      key: 'phone',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Address',
      dataIndex: 'address',
      key: 'address',
      ellipsis: true,
      render: (v: string | null) => v ?? '—',
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
      width: 110,
      render: (_: unknown, record: Branch) => (
        <Space size={4}>
          <Button
            type="link"
            size="small"
            onClick={() => setSelectedBranch(record as unknown as import('@/api/types.gen').Branch)}
            disabled={record.id === (selectedBranch as unknown as Branch)?.id}
          >
            Select
          </Button>
          <Button
            type="text"
            size="small"
            icon={<EditOutlined />}
            onClick={() => { setEditingBranch(record); setModalOpen(true); }}
          />
          <Popconfirm
            title="Delete this branch?"
            onConfirm={() => handleDelete(record.id)}
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
            <BankOutlined style={{ marginRight: 8 }} />
            Branches
          </Title>
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => { setEditingBranch(null); setModalOpen(true); }}
          >
            Add Branch
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Branches</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{branches.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Active</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#3f8600' }}>
              {branches.filter((b) => b.isActive).length}
            </div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>With GSTIN</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#1677ff' }}>
              {branches.filter((b) => b.gstin).length}
            </div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <Input
            placeholder="Search by name, GSTIN or license number…"
            prefix={<SearchOutlined />}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            allowClear
            onClear={() => setSearch('')}
          />
        </Col>
        <Col>
          <Button icon={<ReloadOutlined />} onClick={() => { setSearch(''); branchesQuery.refetch(); }} />
        </Col>
      </Row>

      <Table<Branch>
        rowKey="id"
        columns={columns}
        dataSource={filtered}
        loading={branchesQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} branches` }}
        scroll={{ x: 900 }}
      />

      <BranchFormModal
        open={modalOpen}
        branch={editingBranch}
        onClose={() => setModalOpen(false)}
      />
    </div>
  );
}
