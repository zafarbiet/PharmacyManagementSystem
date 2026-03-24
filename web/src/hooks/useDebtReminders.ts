import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DebtReminder } from '@/api/localTypes';

async function fetchDebtReminders(debtRecordId?: string): Promise<DebtReminder[]> {
  const { data } = await axiosClient.get<DebtReminder[]>('/debt-reminders', {
    params: { debtRecordId },
  });
  return data;
}

export function useDebtReminders(debtRecordId?: string) {
  return useQuery({
    queryKey: ['debt-reminders', debtRecordId],
    queryFn: () => fetchDebtReminders(debtRecordId),
    enabled: !!debtRecordId,
    staleTime: 1000 * 60 * 2,
  });
}
