import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { ExpiryRecord } from '@/api/localTypes';

async function createExpiryRecord(record: Partial<ExpiryRecord>): Promise<ExpiryRecord> {
  const { data } = await axiosClient.post<ExpiryRecord>('/expiry-records', record);
  return data;
}

async function updateExpiryRecord({ id, ...record }: Partial<ExpiryRecord> & { id: string }): Promise<ExpiryRecord> {
  const { data } = await axiosClient.put<ExpiryRecord>(`/expiry-records/${id}`, record);
  return data;
}

async function deleteExpiryRecord(id: string): Promise<void> {
  await axiosClient.delete(`/expiry-records/${id}`);
}

export function useCreateExpiryRecord() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createExpiryRecord,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['expiry-records'] }),
  });
}

export function useUpdateExpiryRecord() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateExpiryRecord,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['expiry-records'] }),
  });
}

export function useDeleteExpiryRecord() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteExpiryRecord,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['expiry-records'] }),
  });
}
