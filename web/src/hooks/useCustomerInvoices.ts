import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { CustomerInvoice } from '@/api/types.gen';

export interface CustomerInvoiceQueryParams {
  patientId?: string;
  status?: string;
  invoiceDateFrom?: string;
  invoiceDateTo?: string;
}

async function fetchCustomerInvoices(params: CustomerInvoiceQueryParams): Promise<CustomerInvoice[]> {
  const { data } = await axiosClient.get<CustomerInvoice[]>('/customer-invoices', { params });
  return data;
}

export function useCustomerInvoices(params: CustomerInvoiceQueryParams = {}) {
  return useQuery({
    queryKey: ['customer-invoices', params],
    queryFn: () => fetchCustomerInvoices(params),
    staleTime: 1000 * 60 * 2,
  });
}
