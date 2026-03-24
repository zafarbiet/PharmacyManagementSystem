import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DebtPayment } from '@/api/localTypes';

async function createDebtPayment(payload: Partial<DebtPayment>): Promise<DebtPayment> {
  const { data } = await axiosClient.post<DebtPayment>('/debt-payments', payload);
  return data;
}

async function deleteDebtPayment(id: string): Promise<void> {
  await axiosClient.delete(`/debt-payments/${id}`);
}

export function useCreateDebtPayment() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createDebtPayment,
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['debt-payments'] });
      qc.invalidateQueries({ queryKey: ['debt-records'] });
    },
  });
}

export function useDeleteDebtPayment() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteDebtPayment,
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['debt-payments'] });
      qc.invalidateQueries({ queryKey: ['debt-records'] });
    },
  });
}
