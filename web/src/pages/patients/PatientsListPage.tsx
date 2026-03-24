import { useState } from 'react';
import {
  Table, Button, Tag, Space, Typography, Row, Col, Card,
  Popconfirm, message, Input, Tooltip,
} from 'antd';
import {
  PlusOutlined, ReloadOutlined, EditOutlined, DeleteOutlined,
  FileTextOutlined, SearchOutlined,
} from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import type { Patient } from '@/api/localTypes';
import { usePatients } from '@/hooks/usePatients';
import { useCreatePatient, useUpdatePatient, useDeletePatient } from '@/hooks/usePatientMutations';
import PatientFormModal from '@/components/PatientFormModal';
import { useNavigate } from 'react-router-dom';

const { Title } = Typography;

export default function PatientsListPage() {
  const [modalState, setModalState] = useState<{ open: boolean; patient: Patient | null }>({
    open: false,
    patient: null,
  });
  const [search, setSearch] = useState('');
  const [queriedSearch, setQueriedSearch] = useState('');

  const patientsQuery = usePatients(queriedSearch || undefined);
  const createPatient = useCreatePatient();
  const updatePatient = useUpdatePatient();
  const deletePatient = useDeletePatient();

  const navigate = useNavigate();

  const patients = patientsQuery.data ?? [];
  const subscriberCount = patients.filter((p) => p.isSubscriber).length;
  const withCredit = patients.filter((p) => p.creditBalance > 0).length;

  const handleSubmit = async (values: Partial<Patient>) => {
    try {
      if (modalState.patient) {
        await updatePatient.mutateAsync({ id: modalState.patient.id, ...values });
        message.success('Patient updated.');
      } else {
        await createPatient.mutateAsync(values);
        message.success('Patient created.');
      }
      setModalState({ open: false, patient: null });
    } catch {
      message.error('Failed to save patient.');
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deletePatient.mutateAsync(id);
      message.success('Patient deleted.');
    } catch {
      message.error('Failed to delete patient.');
    }
  };

  const columns: ColumnsType<Patient> = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      render: (v: string | null) => v ?? '—',
      sorter: (a, b) => (a.name ?? '').localeCompare(b.name ?? ''),
    },
    {
      title: 'Contact',
      dataIndex: 'contactNumber',
      key: 'contactNumber',
      render: (v: string | null) => v ?? <span style={{ color: '#bbb' }}>—</span>,
    },
    {
      title: 'Age',
      dataIndex: 'age',
      key: 'age',
      width: 60,
      render: (v: number | null) => v ?? <span style={{ color: '#bbb' }}>—</span>,
    },
    {
      title: 'GSTIN',
      dataIndex: 'gstin',
      key: 'gstin',
      render: (v: string | null) =>
        v ? (
          <Tag style={{ fontFamily: 'monospace', fontSize: 11 }} color="purple">{v}</Tag>
        ) : (
          <span style={{ color: '#bbb' }}>—</span>
        ),
    },
    {
      title: 'Credit Balance',
      dataIndex: 'creditBalance',
      key: 'creditBalance',
      align: 'right',
      render: (v: number) =>
        v > 0 ? (
          <Tag color="red">
            ₹{v.toLocaleString('en-IN', { minimumFractionDigits: 2 })}
          </Tag>
        ) : (
          <span style={{ color: '#bbb' }}>—</span>
        ),
      sorter: (a, b) => a.creditBalance - b.creditBalance,
    },
    {
      title: 'Type',
      key: 'type',
      render: (_: unknown, record: Patient) =>
        record.isSubscriber ? <Tag color="blue">Subscriber</Tag> : null,
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 120,
      render: (_: unknown, record: Patient) => (
        <Space size={4}>
          <Tooltip title="View Prescriptions">
            <Button
              size="small"
              icon={<FileTextOutlined />}
              onClick={() => navigate(`/patients/${record.id}/prescriptions`)}
            />
          </Tooltip>
          <Button
            size="small"
            icon={<EditOutlined />}
            onClick={() => setModalState({ open: true, patient: record })}
          />
          <Popconfirm
            title="Delete this patient?"
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
          <Title level={4} style={{ margin: 0 }}>Patients</Title>
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => setModalState({ open: true, patient: null })}
          >
            New Patient
          </Button>
        </Col>
      </Row>

      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Total Patients</div>
            <div style={{ fontSize: 22, fontWeight: 700 }}>{patients.length}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>Subscribers</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: '#1677ff' }}>{subscriberCount}</div>
          </Card>
        </Col>
        <Col xs={24} sm={8}>
          <Card size="small">
            <div style={{ fontSize: 12, color: '#888' }}>With Credit Balance</div>
            <div style={{ fontSize: 22, fontWeight: 700, color: withCredit > 0 ? '#cf1322' : '#888' }}>
              {withCredit}
            </div>
          </Card>
        </Col>
      </Row>

      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col>
          <Input
            placeholder="Search by name or phone…"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onPressEnter={() => setQueriedSearch(search)}
            style={{ width: 260 }}
            suffix={
              <SearchOutlined
                style={{ cursor: 'pointer' }}
                onClick={() => setQueriedSearch(search)}
              />
            }
          />
        </Col>
        <Col>
          <Button icon={<ReloadOutlined />} onClick={() => patientsQuery.refetch()} />
        </Col>
      </Row>

      <Table<Patient>
        rowKey="id"
        columns={columns}
        dataSource={patients}
        loading={patientsQuery.isFetching}
        size="small"
        pagination={{ pageSize: 25, showSizeChanger: true, showTotal: (t) => `${t} patients` }}
        scroll={{ x: 800 }}
      />

      <PatientFormModal
        open={modalState.open}
        patient={modalState.patient}
        onClose={() => setModalState({ open: false, patient: null })}
        onSubmit={handleSubmit}
        loading={createPatient.isPending || updatePatient.isPending}
      />
    </div>
  );
}
