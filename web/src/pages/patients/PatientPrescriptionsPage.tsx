import { useState } from 'react';
import {
  Table, Button, Space, Typography, Row, Col,
  Popconfirm, message, Descriptions, Spin, Collapse,
} from 'antd';
import { PlusOutlined, ArrowLeftOutlined, DeleteOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import { useParams, useNavigate } from 'react-router-dom';
import type { Prescription } from '@/api/localTypes';
import { usePatients } from '@/hooks/usePatients';
import { usePrescriptions, usePrescriptionItems } from '@/hooks/usePrescriptions';
import { useCreatePrescription, useDeletePrescription } from '@/hooks/usePrescriptionMutations';
import { useDrugs } from '@/hooks/useDrugs';
import PrescriptionFormModal from '@/components/PrescriptionFormModal';
import { useMemo } from 'react';

const { Title, Text } = Typography;

function PrescriptionItemsPanel({ prescriptionId }: { prescriptionId: string }) {
  const itemsQuery = usePrescriptionItems(prescriptionId);
  const drugsQuery = useDrugs();

  const drugMap = useMemo(() => {
    const m: Record<string, string> = {};
    (drugsQuery.data ?? []).forEach((d) => { if (d.id) m[d.id] = d.name ?? d.id; });
    return m;
  }, [drugsQuery.data]);

  if (itemsQuery.isLoading) return <Spin size="small" />;
  const items = itemsQuery.data ?? [];

  if (items.length === 0) return <Text type="secondary">No items.</Text>;

  return (
    <Table
      dataSource={items}
      rowKey="id"
      size="small"
      pagination={false}
      columns={[
        { title: 'Drug', dataIndex: 'drugId', render: (v: string) => drugMap[v] ?? v },
        { title: 'Dosage', dataIndex: 'dosage', render: (v: string | null) => v ?? '—' },
        { title: 'Qty', dataIndex: 'quantity', width: 60 },
        { title: 'Instructions', dataIndex: 'instructions', render: (v: string | null) => v ?? '—' },
      ]}
    />
  );
}

export default function PatientPrescriptionsPage() {
  const { patientId } = useParams<{ patientId: string }>();
  const navigate = useNavigate();
  const [modalOpen, setModalOpen] = useState(false);

  const patientsQuery = usePatients();
  const patient = (patientsQuery.data ?? []).find((p) => p.id === patientId);
  const rxQuery = usePrescriptions(patientId);
  const createRx = useCreatePrescription();
  const deleteRx = useDeletePrescription();

  const prescriptions = (rxQuery.data ?? []) as Prescription[];

  const handleDelete = async (id: string) => {
    try {
      await deleteRx.mutateAsync(id);
      message.success('Prescription deleted.');
    } catch {
      message.error('Failed to delete prescription.');
    }
  };


  if (!patientId) return null;

  return (
    <div style={{ padding: 24 }}>
      <Row align="middle" style={{ marginBottom: 16 }} gutter={8}>
        <Col>
          <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/patients')} />
        </Col>
        <Col flex="auto">
          <Title level={4} style={{ margin: 0 }}>
            Prescriptions — {patient?.name ?? '…'}
          </Title>
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => setModalOpen(true)}
            disabled={!patient}
          >
            New Prescription
          </Button>
        </Col>
      </Row>

      {patient && (
        <Descriptions size="small" bordered style={{ marginBottom: 16, maxWidth: 600 }} column={2}>
          <Descriptions.Item label="Contact">{patient.contactNumber ?? '—'}</Descriptions.Item>
          <Descriptions.Item label="Age">{patient.age ?? '—'}</Descriptions.Item>
          <Descriptions.Item label="Email" span={2}>{patient.email ?? '—'}</Descriptions.Item>
        </Descriptions>
      )}

      <Collapse
        items={prescriptions.map((rx) => ({
          key: rx.id,
          label: (
            <Space>
              <strong>{dayjs(rx.prescriptionDate).format('DD MMM YYYY')}</strong>
              {rx.prescribingDoctor && <Text type="secondary">Dr. {rx.prescribingDoctor}</Text>}
            </Space>
          ),
          extra: (
            <Popconfirm
              title="Delete this prescription?"
              onConfirm={(e) => { e?.stopPropagation(); handleDelete(rx.id); }}
              okText="Delete"
              okType="danger"
            >
              <Button
                type="text"
                size="small"
                danger
                icon={<DeleteOutlined />}
                onClick={(e) => e.stopPropagation()}
              />
            </Popconfirm>
          ),
          children: <PrescriptionItemsPanel prescriptionId={rx.id} />,
        }))}
      />

      {prescriptions.length === 0 && !rxQuery.isFetching && (
        <div style={{ textAlign: 'center', color: '#888', marginTop: 48 }}>
          No prescriptions on record for this patient.
        </div>
      )}

      {patient && (
        <PrescriptionFormModal
          open={modalOpen}
          patient={patient}
          onClose={() => setModalOpen(false)}
          onSubmit={async (values) => {
            try {
              await createRx.mutateAsync(values as Parameters<typeof createRx.mutateAsync>[0]);
              message.success('Prescription saved.');
              setModalOpen(false);
            } catch {
              message.error('Failed to save prescription.');
            }
          }}
          loading={createRx.isPending}
        />
      )}
    </div>
  );
}
