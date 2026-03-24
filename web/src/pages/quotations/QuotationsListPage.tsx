import { useState, useMemo } from 'react';
import {
  Table, Button, Tag, Space, Typography, Row, Col, Card,
  Popconfirm, message, Select, Modal, Tooltip,
} from 'antd';
import { ReloadOutlined, CheckOutlined, CloseOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { Quotation } from '@/api/localTypes';
import { useQuotations } from '@/hooks/useQuotations';
import { useDeleteQuotation, useAcceptQuotation, useRejectQuotation } from '@/hooks/useQuotationMutations';
import { useVendors } from '@/hooks/useVendors';
import { useQuotationRequests } from '@/hooks/useQuotationRequests';
import { useGlobalStore } from '@/store/globalStore';

const { Title } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Pending: 'blue',
  Accepted: 'green',
  Rejected: 'red',
  Expired: 'default',
};

export default function QuotationsListPage() {
  const [acceptModal, setAcceptModal] = useState<{ open: boolean; quotationId: string | null }>({
    open: false,
    quotationId: null,
  });

  const selectedBranch = useGlobalStore((s) => s.selectedBranch);
  const [overrideBranchId, setOverrideBranchId] = useState<string | undefined>(undefined);

  const quotationsQuery = useQuotations();
  const vendorsQuery = useVendors();
  const rfqQuery = useQuotationRequests();
  const deleteQuotation = useDeleteQuotation();
  const acceptQuotation = useAcceptQuotation();
  const rejectQuotation = useRejectQuotation();

  const vendorMap = useMemo(() => {
    const m: Record<string, string> = {};
    (vendorsQuery.data ?? []).forEach((v) => { if (v.id) m[v.id] = v.name ?? v.id; });
    return m;
  }, [vendorsQuery.data]);

  const rfqMap = useMemo(() => {
    const m: Record<string, string> = {};
    (rfqQuery.data ?? []).forEach((r) => {
      m[r.id] = dayjs(r.requestDate).format('DD MMM YYYY');
    });
    return m;
  }, [rfqQuery.data]);

  const branchOptions = useMemo(() => {
    // For now we only have selectedBranch; expand if branch list hook is added
    if (!selectedBranch) return [];
    return [{ value: selectedBranch.id, label: selectedBranch.name ?? selectedBranch.id }];
  }, [selectedBranch]);

  const quotations = (quotationsQuery.data ?? []) as Quotation[];
  const pendingCount = quotations.filter((q) => q.status === 'Pending').length;
  const acceptedCount = quotations.filter((q) => q.status === 'Accepted').length;

  const openAccept = (id: string) => {
    setOverrideBranchId(selectedBranch?.id);
    setAcceptModal({ open: true, quotationId: id });
  };

  const handleAccept = async () => {
    if (!acceptModal.quotationId) return;
    try {
      await acceptQuotation.mutateAsync({
        id: acceptModal.quotationId,
        branchId: overrideBranchId ?? selectedBranch?.id ?? null,
      });
      message.success('Quotation accepted — Purchase Order created.');
      setAcceptModal({ open: false, quotationId: null });
    } catch {
      message.error('Failed to accept quotation.');
    }
  };

  const handleReject = async (id: string) => {
    try {
      await rejectQuotation.mutateAsync(id);
      message.success('Quotation rejected.');
    } catch {
      message.error('Failed to reject quotation.');
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteQuotation.mutateAsync(id);
      message.success('Quotation deleted.');
    } catch {
      message.error('Failed to delete quotation.');
    }
  };

  const columns: ColumnsType<Quotation> = [
    {
      title: 'Quotation Date',
      dataIndex: 'quotationDate',
      key: 'quotationDate',
      render: (v: string) => dayjs(v).format('DD MMM YYYY'),
      sorter: (a, b) => dayjs(a.quotationDate).unix() - dayjs(b.quotationDate).unix(),
      defaultSortOrder: 'descend',
    },
    {
      title: 'RFQ Date',
      dataIndex: 'quotationRequestId',
      key: 'rfqDate',
      render: (v: string) => rfqMap[v] ?? <span style={{ color: '#bbb' }}>—</span>,
    },
    {
      title: 'Vendor',
      dataIndex: 'vendorId',
      key: 'vendor',
      render: (v: string) => vendorMap[v] ?? <span style={{ color: '#bbb' }}>—</span>,
    },
    {
      title: 'Valid Until',
      dataIndex: 'validUntil',
      key: 'validUntil',
      render: (v: string | null) => {
        if (!v) return <span style={{ color: '#bbb' }}>—</span>;
        const d = dayjs(v);
        const isExpired = d.isBefore(dayjs(), 'day');
        return <Tag color={isExpired ? 'red' : 'green'}>{d.format('DD MMM YYYY')}</Tag>;
      },
    },
    {
      title: 'Total Amount',
      dataIndex: 'totalAmount',
      key: 'totalAmount',
      align: 'right',
      render: (v: number) =>
        v?.toLocaleString('en-IN', { style: 'currency', currency: 'INR' }) ?? '—',
      sorter: (a, b) => a.totalAmount - b.totalAmount,
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: (v: string | null) => <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
      filters: Object.keys(STATUS_COLORS).map((s) => ({ text: s, value: s })),
      onFilter: (value, record) => record.status === value,
    },
    {
      title: 'Notes',
      dataIndex: 'notes',
      key: 'notes',
      ellipsis: true,
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 130,
      render: (_: unknown, record: Quotation) => (
        <Space size={4}>
          {record.status === 'Pending' && (
            <>
              <Tooltip title="Accept & create PO">
                <Button
                  type="primary"
                  size="small"
                  icon={<CheckOutlined />}
                  onClick={() => openAccept(record.id)}
                >
                  Accept
                </Button>
              </Tooltip>
              <Popconfirm
                title="Reject this quotation?"
                onConfirm={() => handleReject(record.id)}
                okText="Reject"
                okType="danger"
              >
                <Button size="small" danger icon={<CloseOutlined />}>
                  Reject
                </Button>
              </Popconfirm>
            </>
          )}
          <Popconfirm
            title="Delete this quotation?"
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
          <Title level={4} style={{ margin: 0 }}>Quotations Received</Title>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Quotations</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{quotations.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Pending Review</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: pendingCount > 0 ? '#1677ff' : '#888' }}>
              {pendingCount}
            </div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Accepted</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#3f8600' }}>{acceptedCount}</div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col>
          <Button icon={<ReloadOutlined />} onClick={() => quotationsQuery.refetch()} />
        </Col>
      </Row>

      <Table<Quotation>
        rowKey="id"
        columns={columns}
        dataSource={quotations}
        loading={quotationsQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} quotations` }}
        scroll={{ x: 900 }}
      />

      <Modal
        title="Accept Quotation"
        open={acceptModal.open}
        onOk={handleAccept}
        onCancel={() => setAcceptModal({ open: false, quotationId: null })}
        okText="Accept & Create PO"
        confirmLoading={acceptQuotation.isPending}
      >
        <div style={{ marginBottom: 8, color: '#888' }}>
          Accepting this quotation will automatically create a Purchase Order.
          Confirm the destination branch:
        </div>
        <Select
          placeholder="Select branch (optional)"
          options={branchOptions}
          value={overrideBranchId}
          onChange={setOverrideBranchId}
          allowClear
          style={{ width: '100%' }}
        />
        {!selectedBranch && (
          <div style={{ marginTop: 8, color: '#d46b08', fontSize: 12 }}>
            No branch selected — PO will not be assigned to a branch.
          </div>
        )}
      </Modal>
    </div>
  );
}
