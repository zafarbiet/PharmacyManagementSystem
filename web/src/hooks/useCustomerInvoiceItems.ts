import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { CustomerInvoiceItem } from '@/api/localTypes';

async function fetchInvoiceItems(invoiceId: string): Promise<CustomerInvoiceItem[]> {
  const { data } = await axiosClient.get<CustomerInvoiceItem[]>('/customer-invoice-items', {
    params: { invoiceId },
  });
  return data;
}

export function useCustomerInvoiceItems(invoiceId: string | undefined) {
  return useQuery({
    queryKey: ['customer-invoice-items', invoiceId],
    queryFn: () => fetchInvoiceItems(invoiceId!),
    enabled: !!invoiceId,
    staleTime: 1000 * 60 * 5,
  });
}
