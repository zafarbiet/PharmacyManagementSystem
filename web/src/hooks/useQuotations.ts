import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Quotation } from '@/api/localTypes';

async function fetchQuotations(quotationRequestId?: string): Promise<Quotation[]> {
  const { data } = await axiosClient.get<Quotation[]>('/quotations', {
    params: quotationRequestId ? { quotationRequestId } : {},
  });
  return data;
}

export function useQuotations(quotationRequestId?: string) {
  return useQuery({
    queryKey: ['quotations', quotationRequestId],
    queryFn: () => fetchQuotations(quotationRequestId),
    staleTime: 1000 * 60 * 2,
  });
}
