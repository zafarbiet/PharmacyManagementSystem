import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DebtRecord } from '@/api/localTypes';

async function createDebtRecord(payload: Partial<DebtRecord>): Promise<DebtRecord> {
  const { data } = await axiosClient.post<DebtRecord>('/debt-records', payload);
  return data;
}

async function updateDebtRecord({ id, ...payload }: Partial<DebtRecord> & { id: string }): Promise<DebtRecord> {
  const { data } = await axiosClient.put<DebtRecord>(`/debt-records/${id}`, payload);
  return data;
}

async function deleteDebtRecord(id: string): Promise<void> {
  await axiosClient.delete(`/debt-records/${id}`);
}

export function useCreateDebtRecord() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createDebtRecord,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['debt-records'] }),
  });
}

export function useUpdateDebtRecord() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateDebtRecord,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['debt-records'] }),
  });
}

export function useDeleteDebtRecord() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteDebtRecord,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['debt-records'] }),
  });
}
