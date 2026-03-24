import { Drawer, Table, Descriptions, Tag, Divider, Button, Spin, Space, Typography } from 'antd';
import { PrinterOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import type { CustomerInvoice, CustomerInvoiceItem } from '@/api/localTypes';
import { useCustomerInvoiceItems } from '@/hooks/useCustomerInvoiceItems';
import { useDrugs } from '@/hooks/useDrugs';
import { useMemo } from 'react';

const { Text } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Draft: 'default', Pending: 'blue', Paid: 'green',
  PartiallyPaid: 'orange', Cancelled: 'red', Refunded: 'purple',
};

function fmt(v: number | undefined) {
  return (v ?? 0).toLocaleString('en-IN', { minimumFractionDigits: 2 });
}

function buildPrintHtml(invoice: CustomerInvoice, items: CustomerInvoiceItem[], drugMap: Map<string, string>): string {
  const rows = items.map((item) => `
    <tr>
      <td>${drugMap.get(item.drugId) ?? item.drugId}</td>
      <td>${item.hsnCode ?? '—'}</td>
      <td>${item.batchNumber ?? '—'}</td>
      <td style="text-align:right">${item.quantity}</td>
      <td style="text-align:right">${fmt(item.unitPrice)}</td>
      <td style="text-align:right">${item.discountPercent ?? 0}%</td>
      <td style="text-align:right">${fmt(item.taxableValue)}</td>
      <td style="text-align:right">${item.gstRate ?? 0}%</td>
      <td style="text-align:right">${fmt(item.cgstAmount)}</td>
      <td style="text-align:right">${fmt(item.sgstAmount)}</td>
      <td style="text-align:right">${fmt(item.igstAmount)}</td>
      <td style="text-align:right"><strong>${fmt(item.amount)}</strong></td>
    </tr>
  `).join('');

  const totalCgst = items.reduce((s, i) => s + (i.cgstAmount ?? 0), 0);
  const totalSgst = items.reduce((s, i) => s + (i.sgstAmount ?? 0), 0);
  const totalIgst = items.reduce((s, i) => s + (i.igstAmount ?? 0), 0);

  return `<!DOCTYPE html><html><head><meta charset="utf-8">
  <title>Invoice ${invoice.invoiceNumber ?? ''}</title>
  <style>
    body { font-family: Arial, sans-serif; font-size: 12px; margin: 24px; color: #222; }
    h2 { margin: 0 0 4px; font-size: 16px; }
    .header { display: flex; justify-content: space-between; margin-bottom: 16px; }
    .meta table { border-collapse: collapse; }
    .meta td { padding: 2px 8px 2px 0; vertical-align: top; }
    .meta td:first-child { color: #666; white-space: nowrap; }
    table.items { width: 100%; border-collapse: collapse; margin-top: 12px; }
    table.items th, table.items td { border: 1px solid #ddd; padding: 4px 6px; }
    table.items th { background: #f5f5f5; text-align: center; font-size: 11px; }
    .totals { margin-top: 12px; text-align: right; }
    .totals table { display: inline-table; border-collapse: collapse; }
    .totals td { padding: 3px 12px; }
    .totals td:first-child { color: #555; }
    .totals td:last-child { text-align: right; font-weight: bold; }
    .footer { margin-top: 40px; font-size: 11px; color: #888; text-align: center; }
    @media print { body { margin: 8px; } }
  </style></head><body>
  <div class="header">
    <div>
      <h2>TAX INVOICE</h2>
      <div><strong>${invoice.invoiceNumber ?? 'N/A'}</strong></div>
      <div>Date: ${invoice.invoiceDate ? dayjs(invoice.invoiceDate).format('DD MMM YYYY') : '—'}</div>
    </div>
    <div class="meta">
      <table>
        <tr><td>Pharmacy GSTIN:</td><td>${invoice.pharmacyGstin ?? '—'}</td></tr>
        <tr><td>Patient GSTIN:</td><td>${invoice.patientGstin ?? '—'}</td></tr>
        <tr><td>Payment:</td><td>${invoice.paymentMethod ?? '—'}</td></tr>
        <tr><td>Billed By:</td><td>${invoice.billedBy ?? '—'}</td></tr>
        <tr><td>Status:</td><td>${invoice.status ?? '—'}</td></tr>
      </table>
    </div>
  </div>
  <table class="items">
    <thead>
      <tr>
        <th>Drug</th><th>HSN</th><th>Batch</th><th>Qty</th>
        <th>Unit Price</th><th>Disc%</th><th>Taxable</th><th>GST%</th>
        <th>CGST</th><th>SGST</th><th>IGST</th><th>Amount</th>
      </tr>
    </thead>
    <tbody>${rows}</tbody>
  </table>
  <div class="totals">
    <table>
      <tr><td>Sub Total:</td><td>₹${fmt(invoice.subTotal)}</td></tr>
      <tr><td>Discount:</td><td>₹${fmt(invoice.discountAmount)}</td></tr>
      <tr><td>CGST:</td><td>₹${fmt(totalCgst)}</td></tr>
      <tr><td>SGST:</td><td>₹${fmt(totalSgst)}</td></tr>
      <tr><td>IGST:</td><td>₹${fmt(totalIgst)}</td></tr>
      <tr style="border-top:2px solid #333;font-size:14px">
        <td>Net Amount:</td><td>₹${fmt(invoice.netAmount)}</td>
      </tr>
    </table>
  </div>
  <div class="footer">This is a computer-generated invoice.</div>
  <script>window.onload = function(){ window.print(); }<\/script>
  </body></html>`;
}

interface Props {
  invoice: CustomerInvoice | null;
  onClose: () => void;
}

export default function InvoiceDetailDrawer({ invoice, onClose }: Props) {
  const itemsQuery = useCustomerInvoiceItems(invoice?.id);
  const drugsQuery = useDrugs();

  const drugMap = useMemo(() => {
    const m = new Map<string, string>();
    (drugsQuery.data ?? []).forEach((d) => { if (d.id) m.set(d.id, d.name ?? d.id); });
    return m;
  }, [drugsQuery.data]);

  const items: CustomerInvoiceItem[] = itemsQuery.data ?? [];

  const totalCgst = items.reduce((s, i) => s + (i.cgstAmount ?? 0), 0);
  const totalSgst = items.reduce((s, i) => s + (i.sgstAmount ?? 0), 0);
  const totalIgst = items.reduce((s, i) => s + (i.igstAmount ?? 0), 0);

  const handlePrint = () => {
    if (!invoice) return;
    const html = buildPrintHtml(invoice, items, drugMap);
    const win = window.open('', '_blank', 'width=900,height=700');
    if (win) {
      win.document.write(html);
      win.document.close();
    }
  };

  const columns: ColumnsType<CustomerInvoiceItem> = [
    {
      title: 'Drug',
      dataIndex: 'drugId',
      render: (id: string) => drugMap.get(id) ?? id,
      ellipsis: true,
    },
    { title: 'HSN', dataIndex: 'hsnCode', render: (v: string | null) => v ?? '—', width: 80 },
    { title: 'Batch', dataIndex: 'batchNumber', render: (v: string | null) => v ?? '—', width: 90 },
    { title: 'Qty', dataIndex: 'quantity', align: 'right', width: 55 },
    { title: 'Unit ₹', dataIndex: 'unitPrice', align: 'right', render: fmt, width: 80 },
    {
      title: 'Disc%',
      dataIndex: 'discountPercent',
      align: 'right',
      width: 60,
      render: (v: number) => v ? `${v}%` : '—',
    },
    { title: 'Taxable', dataIndex: 'taxableValue', align: 'right', render: fmt, width: 85 },
    {
      title: 'GST%',
      dataIndex: 'gstRate',
      align: 'right',
      width: 60,
      render: (v: number) => v ? `${v}%` : '—',
    },
    { title: 'CGST', dataIndex: 'cgstAmount', align: 'right', render: fmt, width: 75 },
    { title: 'SGST', dataIndex: 'sgstAmount', align: 'right', render: fmt, width: 75 },
    { title: 'IGST', dataIndex: 'igstAmount', align: 'right', render: fmt, width: 75 },
    {
      title: 'Amount',
      dataIndex: 'amount',
      align: 'right',
      render: (v: number) => <strong>₹{fmt(v)}</strong>,
      width: 90,
    },
  ];

  return (
    <Drawer
      title={
        <Space>
          <strong>{invoice?.invoiceNumber ?? 'Invoice'}</strong>
          {invoice?.status && (
            <Tag color={STATUS_COLORS[invoice.status] ?? 'default'}>{invoice.status}</Tag>
          )}
        </Space>
      }
      open={!!invoice}
      onClose={onClose}
      width={900}
      extra={
        <Button
          icon={<PrinterOutlined />}
          onClick={handlePrint}
          disabled={items.length === 0}
        >
          Print / PDF
        </Button>
      }
    >
      {invoice && (
        <>
          <Descriptions size="small" bordered column={3} style={{ marginBottom: 16 }}>
            <Descriptions.Item label="Date">
              {invoice.invoiceDate ? dayjs(invoice.invoiceDate).format('DD MMM YYYY') : '—'}
            </Descriptions.Item>
            <Descriptions.Item label="Payment">
              {invoice.paymentMethod ?? '—'}
            </Descriptions.Item>
            <Descriptions.Item label="Billed By">
              {invoice.billedBy ?? '—'}
            </Descriptions.Item>
            <Descriptions.Item label="Pharmacy GSTIN">
              {invoice.pharmacyGstin ?? '—'}
            </Descriptions.Item>
            <Descriptions.Item label="Patient GSTIN">
              {invoice.patientGstin ?? '—'}
            </Descriptions.Item>
            <Descriptions.Item label="Notes">
              {invoice.notes ?? '—'}
            </Descriptions.Item>
          </Descriptions>

          {itemsQuery.isLoading ? (
            <Spin />
          ) : (
            <>
              <Table<CustomerInvoiceItem>
                rowKey="id"
                columns={columns}
                dataSource={items}
                size="small"
                pagination={false}
                scroll={{ x: 900 }}
              />

              <Divider orientation="right" style={{ marginTop: 16 }}>GST Summary</Divider>
              <div style={{ textAlign: 'right', paddingRight: 8 }}>
                <table style={{ display: 'inline-table', borderCollapse: 'collapse' }}>
                  <tbody>
                    <tr>
                      <td style={{ padding: '3px 20px 3px 0', color: '#555' }}>Sub Total:</td>
                      <td style={{ textAlign: 'right' }}>₹{fmt(invoice.subTotal)}</td>
                    </tr>
                    {(invoice.discountAmount ?? 0) > 0 && (
                      <tr>
                        <td style={{ padding: '3px 20px 3px 0', color: '#555' }}>Discount:</td>
                        <td style={{ textAlign: 'right', color: '#cf1322' }}>
                          −₹{fmt(invoice.discountAmount)}
                        </td>
                      </tr>
                    )}
                    <tr>
                      <td style={{ padding: '3px 20px 3px 0', color: '#555' }}>CGST:</td>
                      <td style={{ textAlign: 'right' }}>₹{fmt(totalCgst)}</td>
                    </tr>
                    <tr>
                      <td style={{ padding: '3px 20px 3px 0', color: '#555' }}>SGST:</td>
                      <td style={{ textAlign: 'right' }}>₹{fmt(totalSgst)}</td>
                    </tr>
                    <tr>
                      <td style={{ padding: '3px 20px 3px 0', color: '#555' }}>IGST:</td>
                      <td style={{ textAlign: 'right' }}>₹{fmt(totalIgst)}</td>
                    </tr>
                    <tr style={{ borderTop: '2px solid #333' }}>
                      <td style={{ padding: '6px 20px 3px 0', fontWeight: 700, fontSize: 14 }}>
                        Net Amount:
                      </td>
                      <td style={{ textAlign: 'right', fontWeight: 700, fontSize: 14, color: '#3f8600' }}>
                        ₹{fmt(invoice.netAmount)}
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>

              {items.length === 0 && (
                <Text type="secondary" style={{ display: 'block', textAlign: 'center', marginTop: 24 }}>
                  No line items found for this invoice.
                </Text>
              )}
            </>
          )}
        </>
      )}
    </Drawer>
  );
}
