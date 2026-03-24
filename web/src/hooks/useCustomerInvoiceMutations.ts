import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { CustomerInvoice } from '@/api/localTypes';

export interface InvoiceLineItemPayload {
  drugId: string;
  batchNumber?: string | null;
  quantity: number;
  unitPrice: number;
  discountPercent: number;
}

export interface CreateInvoicePayload {
  invoiceDate: string;
  paymentMethod: string;
  status: string;
  notes?: string | null;
  billedBy?: string | null;
  branchId?: string | null;
  pharmacyGstin?: string | null;
  patientGstin?: string | null;
  items: InvoiceLineItemPayload[];
}

async function createCustomerInvoice(payload: CreateInvoicePayload): Promise<CustomerInvoice> {
  const { data } = await axiosClient.post<CustomerInvoice>('/customer-invoices', payload);
  return data;
}

async function deleteCustomerInvoice(id: string): Promise<void> {
  await axiosClient.delete(`/customer-invoices/${id}`);
}

export function useCreateCustomerInvoice() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createCustomerInvoice,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['customer-invoices'] }),
  });
}

export function useDeleteCustomerInvoice() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteCustomerInvoice,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['customer-invoices'] }),
  });
}
