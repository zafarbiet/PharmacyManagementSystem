import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { ExpiryRecord } from '@/api/localTypes';

export interface ExpiryRecordQueryParams {
  drugInventoryId?: string;
  status?: string;
  dateFrom?: string;
  dateTo?: string;
}

async function fetchExpiryRecords(params: ExpiryRecordQueryParams): Promise<ExpiryRecord[]> {
  const { data } = await axiosClient.get<ExpiryRecord[]>('/expiry-records', { params });
  return data;
}

export function useExpiryRecords(params: ExpiryRecordQueryParams = {}) {
  return useQuery({
    queryKey: ['expiry-records', params],
    queryFn: () => fetchExpiryRecords(params),
    staleTime: 1000 * 60 * 2,
  });
}
