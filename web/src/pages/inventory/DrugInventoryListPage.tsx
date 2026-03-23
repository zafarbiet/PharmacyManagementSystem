import { useState, useMemo } from 'react';
import {
  Table,
  Input,
  Space,
  Typography,
  Button,
  Row,
  Col,
  Statistic,
  Card,
  Tooltip,
} from 'antd';
import { SearchOutlined, ReloadOutlined, PlusOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { useDrugInventory } from '@/hooks/useDrugInventory';
import { useDrugs } from '@/hooks/useDrugs';
import ExpiryTag from '@/components/ExpiryTag';
import type { DrugInventory } from '@/api/localTypes';

const { Title } = Typography;

// Mirrors SaveDrugInventoryAction.DefaultExpiryThresholdDays
const WARN_DAYS = 90;

export default function DrugInventoryListPage() {
  const [batchSearch, setBatchSearch] = useState('');
  const [drugSearch, setDrugSearch] = useState('');
  const [appliedBatch, setAppliedBatch] = useState('');
  const [appliedDrug, setAppliedDrug] = useState('');

  const drugsQuery = useDrugs();
  const inventoryQuery = useDrugInventory();

  const drugMap = useMemo(() => {
    const map = new Map<string, { name: string; mrp: number; reorderLevel: number }>();
    for (const drug of drugsQuery.data ?? []) {
      if (drug.id) {
        map.set(drug.id, {
          name: drug.name ?? '—',
          mrp: drug.mrp ?? 0,
          reorderLevel: drug.reorderLevel ?? 0,
        });
      }
    }
    return map;
  }, [drugsQuery.data]);

  const filteredInventory = useMemo(() => {
    let rows = inventoryQuery.data ?? [];
    if (appliedDrug) {
      const lower = appliedDrug.toLowerCase();
      rows = rows.filter((r) =>
        (drugMap.get(r.drugId)?.name ?? '').toLowerCase().includes(lower),
      );
    }
    if (appliedBatch) {
      const lower = appliedBatch.toLowerCase();
      rows = rows.filter((r) => (r.batchNumber ?? '').toLowerCase().includes(lower));
    }
    return rows;
  }, [inventoryQuery.data, drugMap, appliedDrug, appliedBatch]);

  const stats = useMemo(() => {
    const rows = filteredInventory;
    const now = dayjs();
    return {
      total: rows.length,
      expired: rows.filter((r) => dayjs(r.expirationDate).isBefore(now)).length,
      expiringSoon: rows.filter((r) => {
        const d = dayjs(r.expirationDate).diff(now, 'day');
        return d >= 0 && d <= WARN_DAYS;
      }).length,
      belowReorder: rows.filter((r) => {
        const drug = drugMap.get(r.drugId);
        return drug ? r.quantityInStock <= drug.reorderLevel : false;
      }).length,
    };
  }, [filteredInventory, drugMap]);

  const handleSearch = () => {
    setAppliedBatch(batchSearch.trim());
    setAppliedDrug(drugSearch.trim());
  };

  const handleReset = () => {
    setBatchSearch('');
    setDrugSearch('');
    setAppliedBatch('');
    setAppliedDrug('');
  };

  const columns: ColumnsType<DrugInventory> = [
    {
      title: 'Drug Name',
      dataIndex: 'drugId',
      key: 'drugName',
      width: 200,
      render: (drugId: string) => drugMap.get(drugId)?.name ?? drugId,
      sorter: (a, b) =>
        (drugMap.get(a.drugId)?.name ?? '').localeCompare(drugMap.get(b.drugId)?.name ?? ''),
    },
    {
      title: 'Batch Number',
      dataIndex: 'batchNumber',
      key: 'batchNumber',
      width: 150,
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Expiry Date',
      dataIndex: 'expirationDate',
      key: 'expirationDate',
      width: 220,
      defaultSortOrder: 'ascend',
      sorter: (a, b) => dayjs(a.expirationDate).unix() - dayjs(b.expirationDate).unix(),
      render: (v: string) => <ExpiryTag expirationDate={v} />,
    },
    {
      title: 'Qty in Stock',
      dataIndex: 'quantityInStock',
      key: 'quantityInStock',
      width: 120,
      align: 'right',
      sorter: (a, b) => a.quantityInStock - b.quantityInStock,
      render: (qty: number, record: DrugInventory) => {
        const drug = drugMap.get(record.drugId);
        const isLow = drug ? qty <= drug.reorderLevel : false;
        return (
          <Tooltip title={isLow ? `At or below reorder level (${drug?.reorderLevel})` : undefined}>
            <span style={{ color: isLow ? '#fa8c16' : undefined, fontWeight: isLow ? 600 : undefined }}>
              {qty.toLocaleString('en-IN')}
            </span>
          </Tooltip>
        );
      },
    },
    {
      title: 'MRP (₹)',
      key: 'mrp',
      width: 110,
      align: 'right',
      render: (_: unknown, record: DrugInventory) => {
        const mrp = drugMap.get(record.drugId)?.mrp ?? 0;
        return `₹ ${mrp.toLocaleString('en-IN', { minimumFractionDigits: 2 })}`;
      },
    },
    {
      title: 'Storage Conditions',
      dataIndex: 'storageConditions',
      key: 'storageConditions',
      width: 160,
      render: (v: string | null) => v ?? '—',
    },
    {
      title: 'Last Updated',
      dataIndex: 'updatedAt',
      key: 'updatedAt',
      width: 150,
      render: (v: string) => dayjs(v).format('DD MMM YYYY HH:mm'),
    },
  ];

  return (
    <Space direction="vertical" size={16} style={{ width: '100%' }}>
      <Row justify="space-between" align="middle">
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            Drug Inventory
          </Title>
        </Col>
        <Col>
          <Button type="primary" icon={<PlusOutlined />} disabled>
            Add Batch
          </Button>
        </Col>
      </Row>

      <Row gutter={16}>
        <Col span={6}>
          <Card size="small">
            <Statistic title="Total Batches" value={stats.total} />
          </Card>
        </Col>
        <Col span={6}>
          <Card size="small">
            <Statistic
              title="Expired"
              value={stats.expired}
              valueStyle={{ color: stats.expired > 0 ? '#cf1322' : undefined }}
            />
          </Card>
        </Col>
        <Col span={6}>
          <Card size="small">
            <Statistic
              title="Expiring ≤ 90 days"
              value={stats.expiringSoon}
              valueStyle={{ color: stats.expiringSoon > 0 ? '#d46b08' : undefined }}
            />
          </Card>
        </Col>
        <Col span={6}>
          <Card size="small">
            <Statistic
              title="Below Reorder Level"
              value={stats.belowReorder}
              valueStyle={{ color: stats.belowReorder > 0 ? '#d46b08' : undefined }}
            />
          </Card>
        </Col>
      </Row>

      <Row gutter={12} align="middle">
        <Col>
          <Input
            prefix={<SearchOutlined />}
            placeholder="Search drug name"
            value={drugSearch}
            onChange={(e) => setDrugSearch(e.target.value)}
            onPressEnter={handleSearch}
            style={{ width: 220 }}
            allowClear
            onClear={handleReset}
          />
        </Col>
        <Col>
          <Input
            prefix={<SearchOutlined />}
            placeholder="Search batch number"
            value={batchSearch}
            onChange={(e) => setBatchSearch(e.target.value)}
            onPressEnter={handleSearch}
            style={{ width: 200 }}
            allowClear
            onClear={handleReset}
          />
        </Col>
        <Col>
          <Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>
            Search
          </Button>
        </Col>
        <Col>
          <Button icon={<ReloadOutlined />} onClick={handleReset}>
            Reset
          </Button>
        </Col>
      </Row>

      <Table<DrugInventory>
        columns={columns}
        dataSource={filteredInventory}
        rowKey="id"
        loading={inventoryQuery.isLoading || drugsQuery.isLoading}
        pagination={{
          pageSize: 20,
          showSizeChanger: true,
          pageSizeOptions: ['10', '20', '50'],
          showTotal: (total) => `${total} batches`,
        }}
        scroll={{ x: 1110 }}
        size="small"
        rowClassName={(record) => {
          const now = dayjs();
          const expiry = dayjs(record.expirationDate);
          if (expiry.isBefore(now)) return 'row-expired';
          if (expiry.diff(now, 'day') <= WARN_DAYS) return 'row-expiring';
          return '';
        }}
      />
    </Space>
  );
}
