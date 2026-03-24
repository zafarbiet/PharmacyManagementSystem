import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DebtRecord } from '@/api/localTypes';

async function fetchDebtRecords(patientId?: string, status?: string): Promise<DebtRecord[]> {
  const { data } = await axiosClient.get<DebtRecord[]>('/debt-records', {
    params: { patientId, status },
  });
  return data;
}

export function useDebtRecords(patientId?: string, status?: string) {
  return useQuery({
    queryKey: ['debt-records', patientId, status],
    queryFn: () => fetchDebtRecords(patientId, status),
    staleTime: 1000 * 60 * 2,
  });
}
