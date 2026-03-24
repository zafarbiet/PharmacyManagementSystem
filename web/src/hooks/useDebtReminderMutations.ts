import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DebtReminder } from '@/api/localTypes';

async function createDebtReminder(payload: Partial<DebtReminder>): Promise<DebtReminder> {
  const { data } = await axiosClient.post<DebtReminder>('/debt-reminders', payload);
  return data;
}

async function deleteDebtReminder(id: string): Promise<void> {
  await axiosClient.delete(`/debt-reminders/${id}`);
}

export function useCreateDebtReminder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createDebtReminder,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['debt-reminders'] }),
  });
}

export function useDeleteDebtReminder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteDebtReminder,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['debt-reminders'] }),
  });
}
