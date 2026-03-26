import { useState, useMemo } from 'react';
import {
  Row, Col, Card, Select, InputNumber, Button, Typography, Divider,
  Tag, Space, message, Input, Radio, Tooltip, Badge, Empty, Alert, DatePicker,
  Upload, Spin, Collapse,
} from 'antd';
import {
  DeleteOutlined, ShoppingCartOutlined, UserOutlined,
  MedicineBoxOutlined, PrinterOutlined, ClearOutlined, WarningOutlined,
  UploadOutlined, FileImageOutlined, CheckCircleOutlined, QuestionCircleOutlined,
} from '@ant-design/icons';
import type { UploadFile } from 'antd';
import dayjs from 'dayjs';
import type { Drug } from '@/api/types.gen';
import type { Patient } from '@/api/localTypes';
import { useDrugs } from '@/hooks/useDrugs';
import { usePatients } from '@/hooks/usePatients';
import { useCreateCustomerInvoice } from '@/hooks/useCustomerInvoiceMutations';
import { useCreateDebtRecord } from '@/hooks/useDebtRecordMutations';
import { useGlobalStore } from '@/store/globalStore';
import axiosClient from '@/api/axiosClient';

const { Title, Text } = Typography;

// ── Payment modes ────────────────────────────────────────────────────────────
// 'Partial' is front-desk only; on the wire it becomes 'PartiallyPaid' status
const PAYMENT_MODES = ['Cash', 'UPI', 'Card', 'NEFT', 'Credit', 'Partial'] as const;
type PaymentMode = (typeof PAYMENT_MODES)[number];

const PAYMENT_COLORS: Record<string, string> = {
  Cash: 'green', UPI: 'blue', Card: 'geekblue',
  NEFT: 'cyan', Credit: 'orange', Partial: 'gold',
};

function invoiceStatus(mode: PaymentMode): string {
  if (mode === 'Credit') return 'Pending';
  if (mode === 'Partial') return 'PartiallyPaid';
  return 'Paid';
}

// ── Cart item ────────────────────────────────────────────────────────────────
interface CartItem {
  key: string;
  drug: Drug;
  quantity: number;
  unitPrice: number;
  discountPercent: number;
}

function fmt(v: number) {
  return v.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

function computeItemTotals(item: CartItem) {
  const gross = item.unitPrice * item.quantity;
  const discAmt = gross * (item.discountPercent / 100);
  const taxable = gross - discAmt;
  const gstSlab = item.drug.gstSlab ?? 0;
  const cgst = taxable * (gstSlab / 2) / 100;
  const sgst = cgst;
  return { gross, discAmt, taxable, cgst, sgst, net: taxable + cgst + sgst };
}

// ── Component ────────────────────────────────────────────────────────────────
export default function FrontDeskPage() {
  const [cart, setCart] = useState<CartItem[]>([]);
  const [patientSearch, setPatientSearch] = useState('');
  const [selectedPatient, setSelectedPatient] = useState<Patient | null>(null);
  const [paymentMode, setPaymentMode] = useState<PaymentMode>('Cash');
  const [amountPaid, setAmountPaid] = useState<number>(0);
  const [dueDate, setDueDate] = useState<dayjs.Dayjs | null>(null);
  const [notes, setNotes] = useState('');
  const [drugSearch, setDrugSearch] = useState('');

  // ── Prescription image upload ────────────────────────────────────────────
  const [rxFile, setRxFile] = useState<UploadFile | null>(null);
  const [rxParsing, setRxParsing] = useState(false);
  const [rxExtracted, setRxExtracted] = useState<string[]>([]); // raw names from Claude
  const [rxParsed, setRxParsed] = useState(false);

  const currentUser = useGlobalStore((s) => s.currentUser);
  const selectedBranch = useGlobalStore((s) => s.selectedBranch);

  const drugsQuery = useDrugs({ name: drugSearch || undefined });
  const patientsQuery = usePatients(patientSearch || undefined);
  const createInvoice = useCreateCustomerInvoice();
  const createDebtRecord = useCreateDebtRecord();

  // ── Drug options ─────────────────────────────────────────────────────────
  const drugOptions = useMemo(
    () =>
      (drugsQuery.data ?? [])
        .filter((d) => d.isActive)
        .map((d) => ({
          value: d.id!,
          label: `${d.name ?? d.id}${d.strength ? ` ${d.strength}` : ''}${d.dosageForm ? ` · ${d.dosageForm}` : ''}`,
          drug: d,
        })),
    [drugsQuery.data],
  );

  const patientOptions = useMemo(
    () =>
      (patientsQuery.data ?? [])
        .filter((p) => p.isActive)
        .map((p) => ({
          value: p.id,
          label: `${p.name ?? 'Unknown'} ${p.contactNumber ? `(${p.contactNumber})` : ''}`.trim(),
        })),
    [patientsQuery.data],
  );

  // ── Totals ───────────────────────────────────────────────────────────────
  const totals = useMemo(() => {
    let subTotal = 0, discAmt = 0, cgst = 0, sgst = 0;
    for (const item of cart) {
      const t = computeItemTotals(item);
      subTotal += t.gross;
      discAmt += t.discAmt;
      cgst += t.cgst;
      sgst += t.sgst;
    }
    const net = subTotal - discAmt + cgst + sgst;
    return { subTotal, discAmt, cgst, sgst, net };
  }, [cart]);

  // ── Prescription parse ───────────────────────────────────────────────────
  // For each extracted name, find the best fuzzy match in the loaded drug list
  const rxMatches = useMemo(() => {
    const drugs = drugsQuery.data ?? [];
    return rxExtracted.map((name) => {
      const lower = name.toLowerCase();
      const match = drugs.find(
        (d) =>
          (d.name ?? '').toLowerCase().includes(lower) ||
          lower.includes((d.name ?? '').toLowerCase()) ||
          (d.genericName ?? '').toLowerCase().includes(lower) ||
          lower.includes((d.genericName ?? '').toLowerCase()),
      );
      return { extracted: name, drug: match ?? null };
    });
  }, [rxExtracted, drugsQuery.data]);

  const parseRxImage = async () => {
    if (!rxFile?.originFileObj) return;
    setRxParsing(true);
    setRxExtracted([]);
    setRxParsed(false);
    try {
      const formData = new FormData();
      formData.append('file', rxFile.originFileObj);
      const { data } = await axiosClient.post<{ drugs: string[] }>(
        '/prescriptions/parse-image',
        formData,
        { headers: { 'Content-Type': 'multipart/form-data' } },
      );
      setRxExtracted(data.drugs ?? []);
      setRxParsed(true);
      if ((data.drugs ?? []).length === 0) {
        message.warning('No drug names could be extracted from the image.');
      }
    } catch {
      message.error('Failed to parse prescription image. Check server logs.');
    } finally {
      setRxParsing(false);
    }
  };

  const addRxDrugToCart = (drug: Drug) => {
    const existing = cart.find((i) => i.drug.id === drug.id);
    if (existing) {
      setCart((prev) =>
        prev.map((i) => i.drug.id === drug.id ? { ...i, quantity: i.quantity + 1 } : i),
      );
    } else {
      setCart((prev) => [
        ...prev,
        { key: crypto.randomUUID(), drug, quantity: 1, unitPrice: drug.mrp ?? 0, discountPercent: 0 },
      ]);
    }
  };

  // Derived payment amounts
  const effectiveAmountPaid = paymentMode === 'Credit' ? 0
    : paymentMode === 'Partial' ? amountPaid
    : totals.net;
  const debtAmount = totals.net - effectiveAmountPaid;
  const needsDebt = (paymentMode === 'Credit' || paymentMode === 'Partial') && debtAmount > 0;

  // ── Cart actions ─────────────────────────────────────────────────────────
  const addToCart = (drugId: string) => {
    const drug = (drugsQuery.data ?? []).find((d) => d.id === drugId);
    if (!drug) return;
    const existing = cart.find((i) => i.drug.id === drugId);
    if (existing) {
      setCart((prev) =>
        prev.map((i) => i.drug.id === drugId ? { ...i, quantity: i.quantity + 1 } : i),
      );
    } else {
      setCart((prev) => [
        ...prev,
        { key: crypto.randomUUID(), drug, quantity: 1, unitPrice: drug.mrp ?? 0, discountPercent: 0 },
      ]);
    }
    setDrugSearch('');
  };

  const updateItem = (key: string, field: 'quantity' | 'unitPrice' | 'discountPercent', value: number) => {
    setCart((prev) => prev.map((i) => i.key === key ? { ...i, [field]: value } : i));
  };

  const removeItem = (key: string) => setCart((prev) => prev.filter((i) => i.key !== key));

  const clearSale = () => {
    setCart([]);
    setSelectedPatient(null);
    setNotes('');
    setPaymentMode('Cash');
    setAmountPaid(0);
    setDueDate(null);
  };

  // ── Checkout ─────────────────────────────────────────────────────────────
  const handleCheckout = async () => {
    if (cart.length === 0) { message.warning('Cart is empty.'); return; }

    if (needsDebt && !selectedPatient) {
      message.error('Select a patient for Credit / Partial payment — a debt record will be linked to them.');
      return;
    }

    if (paymentMode === 'Partial') {
      if (amountPaid <= 0) { message.error('Enter the amount being paid now.'); return; }
      if (amountPaid >= totals.net) {
        message.error('Amount paid equals or exceeds total — use Cash/UPI/Card instead.');
        return;
      }
    }

    // Credit-limit guard (client-side, server also enforces)
    const creditLimit = selectedPatient?.creditLimit ?? 0;
    if (paymentMode === 'Credit' && creditLimit > 0 && totals.net > creditLimit) {
      message.error(
        `Total ₹${fmt(totals.net)} exceeds credit limit ₹${fmt(creditLimit)} for ${selectedPatient?.name}.`,
      );
      return;
    }

    try {
      // 1️⃣ Create invoice
      const invoice = await createInvoice.mutateAsync({
        patientId: selectedPatient?.id ?? null,
        invoiceDate: new Date().toISOString(),
        paymentMethod: paymentMode === 'Partial' ? 'Partial' : paymentMode,
        status: invoiceStatus(paymentMode),
        notes: notes || null,
        billedBy: currentUser?.username ?? 'system',
        branchId: selectedBranch?.id ?? null,
        pharmacyGstin: selectedBranch?.gstin ?? null,
        patientGstin: selectedPatient?.gstin ?? null,
        items: cart.map((i) => ({
          drugId: i.drug.id!,
          batchNumber: null,
          quantity: i.quantity,
          unitPrice: i.unitPrice,
          discountPercent: i.discountPercent,
        })),
      });

      // 2️⃣ Create debt record for unpaid portion
      if (needsDebt && selectedPatient) {
        await createDebtRecord.mutateAsync({
          patientId: selectedPatient.id,
          invoiceId: invoice.id,
          originalAmount: totals.net,
          remainingAmount: debtAmount,
          dueDate: dueDate ? dueDate.toISOString() : null,
          status: 'pending',
          notes: paymentMode === 'Partial'
            ? `Partial payment of ₹${fmt(effectiveAmountPaid)} received at billing.`
            : 'Full credit — payment pending.',
        });
      }

      const debtMsg = needsDebt ? ` | Debt ₹${fmt(debtAmount)} recorded.` : '';
      message.success(`Invoice ${invoice.invoiceNumber ?? ''} created — ₹${fmt(totals.net)}${debtMsg}`);
      clearSale();
    } catch (err: unknown) {
      const msg =
        (err as { response?: { data?: { message?: string } } })?.response?.data?.message ??
        'Failed to create invoice.';
      message.error(msg);
    }
  };

  const isBusy = createInvoice.isPending || createDebtRecord.isPending;

  // ── Render ────────────────────────────────────────────────────────────────
  return (
    <div style={{ padding: '16px 20px', height: '100%', display: 'flex', flexDirection: 'column' }}>
      {/* Header */}
      <Row justify="space-between" align="middle" style={{ marginBottom: 12 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            <ShoppingCartOutlined style={{ marginRight: 8, color: '#1677ff' }} />
            Front Desk — New Sale
          </Title>
        </Col>
        <Col>
          {cart.length > 0 && (
            <Button icon={<ClearOutlined />} onClick={clearSale}>Clear Sale</Button>
          )}
        </Col>
      </Row>

      <Row gutter={16} style={{ flex: 1, minHeight: 0 }}>
        {/* ── LEFT: drug search + cart ── */}
        <Col xs={24} lg={15} style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>

          {/* Prescription image upload */}
          <Collapse
            size="small"
            items={[{
              key: 'rx',
              label: (
                <Space>
                  <FileImageOutlined />
                  <span>Upload Prescription (Rx)</span>
                  {rxParsed && rxMatches.length > 0 && (
                    <Tag color="green">{rxMatches.filter((m) => m.drug).length} drug(s) matched</Tag>
                  )}
                </Space>
              ),
              children: (
                <div>
                  <Row gutter={8} align="middle">
                    <Col flex="auto">
                      <Upload
                        accept="image/*"
                        maxCount={1}
                        beforeUpload={() => false}
                        fileList={rxFile ? [rxFile] : []}
                        onChange={({ fileList }) => {
                          setRxFile(fileList[fileList.length - 1] ?? null);
                          setRxExtracted([]);
                          setRxParsed(false);
                        }}
                        showUploadList={{ showRemoveIcon: true }}
                      >
                        <Button icon={<UploadOutlined />} disabled={rxParsing}>
                          Choose Image
                        </Button>
                      </Upload>
                    </Col>
                    <Col>
                      <Button
                        type="primary"
                        onClick={parseRxImage}
                        disabled={!rxFile}
                        loading={rxParsing}
                      >
                        {rxParsing ? 'Reading…' : 'Read Prescription'}
                      </Button>
                    </Col>
                  </Row>

                  {rxParsing && (
                    <div style={{ textAlign: 'center', padding: '16px 0' }}>
                      <Spin size="small" />
                      <div style={{ fontSize: 12, color: '#888', marginTop: 6 }}>
                        Analysing prescription with AI…
                      </div>
                    </div>
                  )}

                  {rxParsed && rxMatches.length > 0 && (
                    <div style={{ marginTop: 12 }}>
                      <div style={{ fontSize: 12, color: '#595959', marginBottom: 6, fontWeight: 600 }}>
                        Extracted drugs — click a match to add to cart:
                      </div>
                      <Space direction="vertical" style={{ width: '100%' }} size={4}>
                        {rxMatches.map(({ extracted, drug }, i) => (
                          <div key={i} style={{
                            display: 'flex', alignItems: 'center', justifyContent: 'space-between',
                            padding: '6px 10px',
                            background: drug ? '#f6ffed' : '#fffbe6',
                            border: `1px solid ${drug ? '#b7eb8f' : '#ffe58f'}`,
                            borderRadius: 6,
                          }}>
                            <Space size={6}>
                              {drug
                                ? <CheckCircleOutlined style={{ color: '#52c41a' }} />
                                : <QuestionCircleOutlined style={{ color: '#faad14' }} />}
                              <div>
                                <div style={{ fontSize: 13, fontWeight: 500 }}>
                                  {drug ? drug.name : extracted}
                                </div>
                                {drug && drug.name !== extracted && (
                                  <div style={{ fontSize: 11, color: '#888' }}>
                                    from Rx: "{extracted}"
                                  </div>
                                )}
                                {!drug && (
                                  <div style={{ fontSize: 11, color: '#faad14' }}>
                                    Not found in catalog — add manually
                                  </div>
                                )}
                              </div>
                            </Space>
                            {drug && (
                              <Button
                                size="small"
                                type="primary"
                                ghost
                                onClick={() => addRxDrugToCart(drug)}
                              >
                                + Add
                              </Button>
                            )}
                          </div>
                        ))}
                      </Space>
                    </div>
                  )}

                  {rxParsed && rxMatches.length === 0 && (
                    <Alert
                      type="warning"
                      message="No drugs detected in the image."
                      description="Try a clearer photo. You can still add drugs manually using the search above."
                      style={{ marginTop: 10, fontSize: 12 }}
                      showIcon
                    />
                  )}
                </div>
              ),
            }]}
          />

          <Card size="small" bodyStyle={{ padding: '10px 12px' }}>
            <Select
              showSearch
              placeholder="Search drug by name, brand or strength…"
              options={drugOptions}
              value={null}
              filterOption={false}
              onSearch={setDrugSearch}
              onChange={addToCart}
              loading={drugsQuery.isFetching}
              style={{ width: '100%' }}
              size="large"
              suffixIcon={<MedicineBoxOutlined />}
              notFoundContent={drugSearch.length >= 2 ? 'No drugs found' : 'Type to search…'}
            />
          </Card>

          <Card
            size="small"
            title={
              <Space>
                Cart
                <Badge count={cart.length} style={{ backgroundColor: '#1677ff' }} />
              </Space>
            }
            style={{ flex: 1 }}
            bodyStyle={{ padding: 0 }}
          >
            {cart.length === 0 ? (
              <div style={{ padding: 32 }}>
                <Empty
                  image={Empty.PRESENTED_IMAGE_SIMPLE}
                  description="Search and select a drug above to add it to the cart"
                />
              </div>
            ) : (
              <div style={{ overflowX: 'auto' }}>
                <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                  <thead>
                    <tr style={{ background: '#fafafa', borderBottom: '1px solid #f0f0f0' }}>
                      {['Drug', 'MRP', 'Qty', 'Unit Price (₹)', 'Disc %', 'GST', 'Amount', ''].map((h) => (
                        <th key={h} style={{
                          padding: '8px 10px', textAlign: h === 'Amount' ? 'right' : 'left',
                          fontSize: 12, fontWeight: 600, color: '#595959', whiteSpace: 'nowrap',
                        }}>
                          {h}
                        </th>
                      ))}
                    </tr>
                  </thead>
                  <tbody>
                    {cart.map((item, idx) => {
                      const t = computeItemTotals(item);
                      const isScheduleH = item.drug.scheduleCategory &&
                        ['H', 'H1', 'X'].includes(item.drug.scheduleCategory);
                      return (
                        <tr key={item.key} style={{
                          borderBottom: '1px solid #f0f0f0',
                          background: idx % 2 === 0 ? '#fff' : '#fafafa',
                        }}>
                          <td style={{ padding: '6px 10px', maxWidth: 200 }}>
                            <div style={{ fontWeight: 500, fontSize: 13 }}>
                              {item.drug.name}
                              {isScheduleH && (
                                <Tag color="red" style={{ marginLeft: 6, fontSize: 10 }}>
                                  Sch {item.drug.scheduleCategory}
                                </Tag>
                              )}
                            </div>
                            {item.drug.strength && (
                              <div style={{ fontSize: 11, color: '#888' }}>{item.drug.strength}</div>
                            )}
                          </td>
                          <td style={{ padding: '6px 10px', color: '#888', fontSize: 12, whiteSpace: 'nowrap' }}>
                            ₹{fmt(item.drug.mrp ?? 0)}
                          </td>
                          <td style={{ padding: '6px 6px' }}>
                            <InputNumber min={1} value={item.quantity}
                              onChange={(v) => updateItem(item.key, 'quantity', v ?? 1)}
                              style={{ width: 64 }} size="small" />
                          </td>
                          <td style={{ padding: '6px 6px' }}>
                            <InputNumber min={0} max={item.drug.mrp ?? undefined} precision={2}
                              value={item.unitPrice}
                              onChange={(v) => updateItem(item.key, 'unitPrice', v ?? 0)}
                              style={{ width: 90 }} size="small" />
                          </td>
                          <td style={{ padding: '6px 6px' }}>
                            <InputNumber min={0} max={100} precision={1}
                              value={item.discountPercent}
                              onChange={(v) => updateItem(item.key, 'discountPercent', v ?? 0)}
                              style={{ width: 70 }} size="small" />
                          </td>
                          <td style={{ padding: '6px 10px', textAlign: 'center' }}>
                            {item.drug.gstSlab
                              ? <Tag color="blue" style={{ fontSize: 11 }}>{item.drug.gstSlab}%</Tag>
                              : <span style={{ color: '#ccc' }}>—</span>}
                          </td>
                          <td style={{ padding: '6px 10px', textAlign: 'right', whiteSpace: 'nowrap' }}>
                            <strong>₹{fmt(t.net)}</strong>
                            {t.discAmt > 0 && (
                              <div style={{ fontSize: 10, color: '#cf1322' }}>−₹{fmt(t.discAmt)}</div>
                            )}
                          </td>
                          <td style={{ padding: '6px 6px', textAlign: 'center' }}>
                            <Button type="text" danger size="small"
                              icon={<DeleteOutlined />} onClick={() => removeItem(item.key)} />
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>

                {/* Cart totals strip */}
                <div style={{
                  padding: '10px 14px', background: '#f6ffed', borderTop: '2px solid #b7eb8f',
                  display: 'flex', justifyContent: 'flex-end', gap: 24, flexWrap: 'wrap',
                }}>
                  <Text type="secondary" style={{ fontSize: 12 }}>
                    Sub Total: <strong>₹{fmt(totals.subTotal)}</strong>
                  </Text>
                  {totals.discAmt > 0 && (
                    <Text style={{ fontSize: 12, color: '#cf1322' }}>
                      Discount: <strong>−₹{fmt(totals.discAmt)}</strong>
                    </Text>
                  )}
                  <Text style={{ fontSize: 12, color: '#1677ff' }}>
                    CGST: <strong>₹{fmt(totals.cgst)}</strong>
                  </Text>
                  <Text style={{ fontSize: 12, color: '#1677ff' }}>
                    SGST: <strong>₹{fmt(totals.sgst)}</strong>
                  </Text>
                  <Text strong style={{ fontSize: 15, color: '#3f8600' }}>
                    Net: ₹{fmt(totals.net)}
                  </Text>
                </div>
              </div>
            )}
          </Card>
        </Col>

        {/* ── RIGHT: patient + payment + checkout ── */}
        <Col xs={24} lg={9} style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>

          {/* Patient */}
          <Card size="small" title={<><UserOutlined style={{ marginRight: 6 }} />Patient</>}
            extra={needsDebt && !selectedPatient
              ? <Tag color="red" icon={<WarningOutlined />}>Required</Tag>
              : null}>
            <Select
              showSearch allowClear
              placeholder="Search by name or phone…"
              options={patientOptions}
              value={selectedPatient?.id ?? null}
              filterOption={false}
              onSearch={setPatientSearch}
              onChange={(v) => {
                const p = (patientsQuery.data ?? []).find((x) => x.id === v) ?? null;
                setSelectedPatient(p);
              }}
              onClear={() => setSelectedPatient(null)}
              loading={patientsQuery.isFetching}
              style={{ width: '100%' }}
            />
            {selectedPatient && (
              <div style={{ marginTop: 8, padding: '8px 10px', background: '#f5f5f5', borderRadius: 6 }}>
                <Row gutter={8}>
                  <Col span={12}>
                    <Text style={{ fontSize: 11, color: '#888' }}>Phone</Text>
                    <div style={{ fontSize: 13 }}>{selectedPatient.contactNumber ?? '—'}</div>
                  </Col>
                  <Col span={12}>
                    <Text style={{ fontSize: 11, color: '#888' }}>Credit Limit</Text>
                    <div>
                      {selectedPatient.creditLimit > 0
                        ? <Tag color={totals.net > selectedPatient.creditLimit ? 'red' : 'green'}>
                            ₹{fmt(selectedPatient.creditLimit)}
                          </Tag>
                        : <span style={{ color: '#aaa', fontSize: 12 }}>Unlimited</span>}
                    </div>
                  </Col>
                  {selectedPatient.isSubscriber && (
                    <Col span={24} style={{ marginTop: 4 }}>
                      <Tag color="purple">Subscriber</Tag>
                    </Col>
                  )}
                </Row>
              </div>
            )}
          </Card>

          {/* Payment mode */}
          <Card size="small" title="Payment">
            <Radio.Group
              value={paymentMode}
              onChange={(e) => {
                setPaymentMode(e.target.value as PaymentMode);
                setAmountPaid(0);
              }}
              style={{ width: '100%' }}
            >
              <Row gutter={[8, 8]}>
                {PAYMENT_MODES.map((mode) => (
                  <Col span={8} key={mode}>
                    <Radio.Button
                      value={mode}
                      style={{
                        width: '100%',
                        textAlign: 'center',
                        paddingInline: 4,
                        borderColor: paymentMode === mode ? undefined : '#d9d9d9',
                      }}
                    >
                      <Tag color={PAYMENT_COLORS[mode]} style={{ margin: 0, fontSize: 11 }}>{mode}</Tag>
                    </Radio.Button>
                  </Col>
                ))}
              </Row>
            </Radio.Group>

            {/* Partial payment input */}
            {paymentMode === 'Partial' && (
              <div style={{ marginTop: 12 }}>
                <Text style={{ fontSize: 12, color: '#595959' }}>Amount Paid Now (₹)</Text>
                <InputNumber
                  min={0}
                  max={totals.net - 0.01}
                  precision={2}
                  value={amountPaid}
                  onChange={(v) => setAmountPaid(v ?? 0)}
                  style={{ width: '100%', marginTop: 4 }}
                  placeholder="0.00"
                />
                {amountPaid > 0 && amountPaid < totals.net && (
                  <div style={{ marginTop: 6, display: 'flex', justifyContent: 'space-between' }}>
                    <Text style={{ fontSize: 12, color: '#3f8600' }}>
                      Received: <strong>₹{fmt(amountPaid)}</strong>
                    </Text>
                    <Text style={{ fontSize: 12, color: '#cf1322' }}>
                      Debt: <strong>₹{fmt(debtAmount)}</strong>
                    </Text>
                  </div>
                )}
              </div>
            )}

            {/* Due date for Credit / Partial */}
            {(paymentMode === 'Credit' || paymentMode === 'Partial') && (
              <div style={{ marginTop: 10 }}>
                <Text style={{ fontSize: 12, color: '#595959' }}>Due Date (optional)</Text>
                <DatePicker
                  value={dueDate}
                  onChange={setDueDate}
                  style={{ width: '100%', marginTop: 4 }}
                  format="DD MMM YYYY"
                  disabledDate={(d) => d.isBefore(dayjs(), 'day')}
                />
              </div>
            )}
          </Card>

          {/* Debt preview */}
          {needsDebt && (
            <Alert
              type={paymentMode === 'Credit' ? 'warning' : 'info'}
              icon={<WarningOutlined />}
              showIcon
              message={
                paymentMode === 'Credit'
                  ? `Full amount ₹${fmt(totals.net)} will be recorded as debt.`
                  : `₹${fmt(debtAmount)} will be recorded as outstanding debt.`
              }
              description={!selectedPatient ? 'Select a patient to link the debt record.' : undefined}
              style={{ fontSize: 12 }}
            />
          )}

          {/* Notes */}
          <Card size="small" title="Notes">
            <Input.TextArea rows={2} placeholder="Optional notes…" value={notes}
              onChange={(e) => setNotes(e.target.value)} />
          </Card>

          {/* Bill summary */}
          <Card size="small" title="Bill Summary"
            style={{ background: '#f0f9eb', borderColor: '#b7eb8f' }}>
            <Space direction="vertical" style={{ width: '100%' }} size={4}>
              <Row justify="space-between">
                <Col><Text type="secondary">Items</Text></Col>
                <Col><Text>{cart.reduce((s, i) => s + i.quantity, 0)}</Text></Col>
              </Row>
              <Row justify="space-between">
                <Col><Text type="secondary">Sub Total</Text></Col>
                <Col><Text>₹{fmt(totals.subTotal)}</Text></Col>
              </Row>
              {totals.discAmt > 0 && (
                <Row justify="space-between">
                  <Col><Text type="secondary">Discount</Text></Col>
                  <Col><Text style={{ color: '#cf1322' }}>−₹{fmt(totals.discAmt)}</Text></Col>
                </Row>
              )}
              <Row justify="space-between">
                <Col>
                  <Tooltip title={`CGST ₹${fmt(totals.cgst)} + SGST ₹${fmt(totals.sgst)}`}>
                    <Text type="secondary" style={{ cursor: 'default', textDecoration: 'underline dotted' }}>
                      GST
                    </Text>
                  </Tooltip>
                </Col>
                <Col><Text style={{ color: '#1677ff' }}>₹{fmt(totals.cgst + totals.sgst)}</Text></Col>
              </Row>
              <Divider style={{ margin: '6px 0' }} />
              <Row justify="space-between">
                <Col><Text strong style={{ fontSize: 15 }}>Net Payable</Text></Col>
                <Col><Text strong style={{ fontSize: 18, color: '#3f8600' }}>₹{fmt(totals.net)}</Text></Col>
              </Row>
              {paymentMode === 'Partial' && amountPaid > 0 && (
                <>
                  <Row justify="space-between">
                    <Col><Text style={{ fontSize: 13, color: '#3f8600' }}>Paid Now</Text></Col>
                    <Col><Text style={{ fontSize: 13, color: '#3f8600' }}>₹{fmt(amountPaid)}</Text></Col>
                  </Row>
                  <Row justify="space-between">
                    <Col><Text style={{ fontSize: 13, color: '#cf1322' }}>Remaining Debt</Text></Col>
                    <Col><Text strong style={{ fontSize: 13, color: '#cf1322' }}>₹{fmt(debtAmount)}</Text></Col>
                  </Row>
                </>
              )}
              {paymentMode === 'Credit' && totals.net > 0 && (
                <Row justify="space-between">
                  <Col><Text style={{ fontSize: 13, color: '#cf1322' }}>On Credit</Text></Col>
                  <Col><Text strong style={{ fontSize: 13, color: '#cf1322' }}>₹{fmt(totals.net)}</Text></Col>
                </Row>
              )}
            </Space>
          </Card>

          {/* Checkout */}
          <Space direction="vertical" style={{ width: '100%' }}>
            <Button
              type="primary"
              size="large"
              block
              onClick={handleCheckout}
              loading={isBusy}
              disabled={cart.length === 0}
              style={{ height: 48, fontSize: 16 }}
              danger={needsDebt}
            >
              {needsDebt
                ? `Checkout + Record Debt ₹${fmt(debtAmount)}`
                : `Checkout — ₹${fmt(totals.net)}`}
            </Button>
            <Button size="small" block icon={<PrinterOutlined />} disabled style={{ color: '#888' }}>
              Print Receipt (after checkout)
            </Button>
          </Space>
        </Col>
      </Row>
    </div>
  );
}
