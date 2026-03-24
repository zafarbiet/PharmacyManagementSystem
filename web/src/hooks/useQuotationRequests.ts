import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { QuotationRequest } from '@/api/localTypes';

async function fetchQuotationRequests(): Promise<QuotationRequest[]> {
  const { data } = await axiosClient.get<QuotationRequest[]>('/quotation-requests');
  return data;
}

export function useQuotationRequests() {
  return useQuery({
    queryKey: ['quotation-requests'],
    queryFn: fetchQuotationRequests,
    staleTime: 1000 * 60 * 2,
  });
}
