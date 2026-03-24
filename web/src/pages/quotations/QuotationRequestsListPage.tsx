import { useState, useMemo } from 'react';
import {
  Table, Button, Tag, Space, Typography, Row, Col, Card,
  Popconfirm, message, Modal, Select, Tooltip,
} from 'antd';
import { PlusOutlined, ReloadOutlined, SendOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { QuotationRequest } from '@/api/localTypes';
import { useQuotationRequests } from '@/hooks/useQuotationRequests';
import { useDeleteQuotationRequest, useDispatchQuotationRequest } from '@/hooks/useQuotationRequestMutations';
import { useVendors } from '@/hooks/useVendors';
import QuotationRequestFormModal from '@/components/QuotationRequestFormModal';

const { Title } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Draft: 'default',
  Sent: 'blue',
  Closed: 'green',
  Cancelled: 'red',
};

export default function QuotationRequestsListPage() {
  const [modalOpen, setModalOpen] = useState(false);
  const [dispatchModal, setDispatchModal] = useState<{ open: boolean; rfqId: string | null }>({ open: false, rfqId: null });
  const [selectedVendorIds, setSelectedVendorIds] = useState<string[]>([]);

  const rfqQuery = useQuotationRequests();
  const vendorsQuery = useVendors();
  const deleteRFQ = useDeleteQuotationRequest();
  const dispatchRFQ = useDispatchQuotationRequest();

  const vendorOptions = useMemo(
    () => (vendorsQuery.data ?? []).filter((v) => v.isActive).map((v) => ({ value: v.id, label: v.name ?? v.id })),
    [vendorsQuery.data],
  );

  const rfqs = (rfqQuery.data ?? []) as QuotationRequest[];

  const openDispatch = (id: string) => {
    setSelectedVendorIds([]);
    setDispatchModal({ open: true, rfqId: id });
  };

  const handleDispatch = async () => {
    if (!dispatchModal.rfqId || selectedVendorIds.length === 0) {
      message.warning('Select at least one vendor.');
      return;
    }
    try {
      await dispatchRFQ.mutateAsync({ id: dispatchModal.rfqId, vendorIds: selectedVendorIds });
      message.success(`RFQ dispatched to ${selectedVendorIds.length} vendor(s).`);
      setDispatchModal({ open: false, rfqId: null });
    } catch {
      message.error('Failed to dispatch RFQ.');
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteRFQ.mutateAsync(id);
      message.success('RFQ deleted.');
    } catch {
      message.error('Failed to delete RFQ.');
    }
  };

  const draftCount = rfqs.filter((r) => r.status === 'Draft').length;
  const sentCount = rfqs.filter((r) => r.status === 'Sent').length;

  const columns: ColumnsType<QuotationRequest> = [
    {
      title: 'Request Date',
      dataIndex: 'requestDate',
      key: 'requestDate',
      render: (v: string) => dayjs(v).format('DD MMM YYYY'),
      sorter: (a, b) => dayjs(a.requestDate).unix() - dayjs(b.requestDate).unix(),
      defaultSortOrder: 'descend',
    },
    {
      title: 'Required By',
      dataIndex: 'requiredByDate',
      key: 'requiredByDate',
      render: (v: string | null) => {
        if (!v) return <span style={{ color: '#bbb' }}>—</span>;
        const d = dayjs(v);
        const isOverdue = d.isBefore(dayjs(), 'day');
        return <Tag color={isOverdue ? 'red' : 'orange'}>{d.format('DD MMM YYYY')}</Tag>;
      },
      sorter: (a, b) => dayjs(a.requiredByDate ?? 0).unix() - dayjs(b.requiredByDate ?? 0).unix(),
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: (v: string | null) => <Tag color={STATUS_COLORS[v ?? ''] ?? 'default'}>{v ?? '—'}</Tag>,
    },
    {
      title: 'Requested By',
      dataIndex: 'requestedBy',
      key: 'requestedBy',
      render: (v: string | null) => v ?? '—',
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
      width: 110,
      render: (_: unknown, record: QuotationRequest) => (
        <Space size={4}>
          {record.status === 'Draft' && (
            <Tooltip title="Dispatch to vendors">
              <Button
                type="primary"
                size="small"
                icon={<SendOutlined />}
                onClick={() => openDispatch(record.id)}
              >
                Dispatch
              </Button>
            </Tooltip>
          )}
          <Popconfirm
            title="Delete this RFQ?"
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
          <Title level={4} style={{ margin: 0 }}>Quotation Requests (RFQ)</Title>
        </Col>
        <Col>
          <Button type="primary" icon={<PlusOutlined />} onClick={() => setModalOpen(true)}>
            New RFQ
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total RFQs</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{rfqs.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Draft</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: draftCount > 0 ? '#d46b08' : '#888' }}>{draftCount}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Sent to Vendors</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#1677ff' }}>{sentCount}</div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col>
          <Button icon={<ReloadOutlined />} onClick={() => rfqQuery.refetch()} />
        </Col>
      </Row>

      <Table<QuotationRequest>
        rowKey="id"
        columns={columns}
        dataSource={rfqs}
        loading={rfqQuery.isFetching}
        size="small"
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `${t} requests` }}
        scroll={{ x: 800 }}
      />

      <QuotationRequestFormModal open={modalOpen} onClose={() => setModalOpen(false)} />

      <Modal
        title="Dispatch RFQ to Vendors"
        open={dispatchModal.open}
        onOk={handleDispatch}
        onCancel={() => setDispatchModal({ open: false, rfqId: null })}
        okText="Dispatch"
        confirmLoading={dispatchRFQ.isPending}
      >
        <div style={{ marginBottom: 8, color: '#888' }}>
          Select the vendors to send this request for quotation to:
        </div>
        <Select
          mode="multiple"
          placeholder="Select vendors…"
          options={vendorOptions}
          value={selectedVendorIds}
          onChange={setSelectedVendorIds}
          style={{ width: '100%' }}
          loading={vendorsQuery.isLoading}
          filterOption={(i, o) => (o?.label ?? '').toLowerCase().includes(i.toLowerCase())}
        />
      </Modal>
    </div>
  );
}
