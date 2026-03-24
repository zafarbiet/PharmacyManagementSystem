import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DebtPayment } from '@/api/localTypes';

async function fetchDebtPayments(debtRecordId?: string): Promise<DebtPayment[]> {
  const { data } = await axiosClient.get<DebtPayment[]>('/debt-payments', {
    params: { debtRecordId },
  });
  return data;
}

export function useDebtPayments(debtRecordId?: string) {
  return useQuery({
    queryKey: ['debt-payments', debtRecordId],
    queryFn: () => fetchDebtPayments(debtRecordId),
    enabled: !!debtRecordId,
    staleTime: 1000 * 60 * 2,
  });
}
