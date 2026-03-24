import { useState, useMemo } from 'react';
import { Table, Input, Button, Tag, Space, Typography, Row, Col, Card, Popconfirm, message } from 'antd';
import { SearchOutlined, PlusOutlined, ReloadOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import type { Vendor } from '@/api/types.gen';
import { useVendors } from '@/hooks/useVendors';
import { useDeleteVendor } from '@/hooks/useVendorMutations';
import VendorFormModal from '@/components/VendorFormModal';

const { Title } = Typography;

export default function VendorsListPage() {
  const [search, setSearch] = useState('');
  const [appliedSearch, setAppliedSearch] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingVendor, setEditingVendor] = useState<Vendor | null>(null);

  const vendorsQuery = useVendors();
  const deleteVendor = useDeleteVendor();

  const filtered = useMemo(() => {
    const vendors = vendorsQuery.data ?? [];
    if (!appliedSearch) return vendors;
    const lower = appliedSearch.toLowerCase();
    return vendors.filter(
      (v) =>
        (v.name ?? '').toLowerCase().includes(lower) ||
        (v.contactPerson ?? '').toLowerCase().includes(lower) ||
        (v.phone ?? '').toLowerCase().includes(lower) ||
        (v.gstNumber ?? '').toLowerCase().includes(lower),
    );
  }, [vendorsQuery.data, appliedSearch]);

  const handleAdd = () => {
    setEditingVendor(null);
    setModalOpen(true);
  };

  const handleEdit = (vendor: Vendor) => {
    setEditingVendor(vendor);
    setModalOpen(true);
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteVendor.mutateAsync(id);
      message.success('Vendor deleted.');
    } catch {
      message.error('Failed to delete vendor.');
    }
  };

  const columns: ColumnsType<Vendor> = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => (a.name ?? '').localeCompare(b.name ?? ''),
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Contact Person',
      dataIndex: 'contactPerson',
      key: 'contactPerson',
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
      title: 'GST Number',
      dataIndex: 'gstNumber',
      key: 'gstNumber',
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Credit Terms',
      dataIndex: 'creditTermsDays',
      key: 'creditTermsDays',
      render: (days: number) => `${days ?? 0} days`,
      sorter: (a, b) => (a.creditTermsDays ?? 0) - (b.creditTermsDays ?? 0),
    },
    {
      title: 'Credit Limit',
      dataIndex: 'creditLimit',
      key: 'creditLimit',
      align: 'right',
      render: (v: number) =>
        (v ?? 0).toLocaleString('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 }),
      sorter: (a, b) => (a.creditLimit ?? 0) - (b.creditLimit ?? 0),
    },
    {
      title: 'Outstanding',
      dataIndex: 'outstandingBalance',
      key: 'outstandingBalance',
      align: 'right',
      render: (v: number) => {
        const amount = v ?? 0;
        const formatted = amount.toLocaleString('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 });
        return amount > 0 ? <Tag color="red">{formatted}</Tag> : <Tag color="green">₹0</Tag>;
      },
      sorter: (a, b) => (a.outstandingBalance ?? 0) - (b.outstandingBalance ?? 0),
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
      width: 100,
      render: (_: unknown, record: Vendor) => (
        <Space size={4}>
          <Button
            type="text"
            size="small"
            icon={<EditOutlined />}
            onClick={() => handleEdit(record)}
          />
          <Popconfirm
            title="Delete this vendor?"
            description="This will soft-delete the vendor record."
            onConfirm={() => handleDelete(record.id!)}
            okText="Delete"
            okType="danger"
          >
            <Button
              type="text"
              size="small"
              danger
              icon={<DeleteOutlined />}
              loading={deleteVendor.isPending}
            />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  const vendors = vendorsQuery.data ?? [];
  const totalCredit = vendors.reduce((s, v) => s + (v.creditLimit ?? 0), 0);
  const totalOutstanding = vendors.reduce((s, v) => s + (v.outstandingBalance ?? 0), 0);

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>Vendors</Title>
        </Col>
        <Col>
          <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd}>
            Add Vendor
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Vendors</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{vendors.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Credit Limit</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>
              {totalCredit.toLocaleString('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 })}
            </div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Outstanding</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: totalOutstanding > 0 ? '#cf1322' : '#3f8600' }}>
              {totalOutstanding.toLocaleString('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 })}
            </div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <Input
            placeholder="Search by name, contact, phone or GST…"
            prefix={<SearchOutlined />}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onPressEnter={() => setAppliedSearch(search)}
            allowClear
            onClear={() => { setSearch(''); setAppliedSearch(''); }}
          />
        </Col>
        <Col>
          <Space>
            <Button type="primary" icon={<SearchOutlined />} onClick={() => setAppliedSearch(search)}>
              Search
            </Button>
            <Button
              icon={<ReloadOutlined />}
              onClick={() => { setSearch(''); setAppliedSearch(''); vendorsQuery.refetch(); }}
            />
          </Space>
        </Col>
      </Row>

      <Table<Vendor>
        rowKey="id"
        columns={columns}
        dataSource={filtered}
        loading={vendorsQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} vendors` }}
        scroll={{ x: 1000 }}
      />

      <VendorFormModal
        open={modalOpen}
        vendor={editingVendor}
        onClose={() => setModalOpen(false)}
      />
    </div>
  );
}
