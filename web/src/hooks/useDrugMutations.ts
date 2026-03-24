import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Drug } from '@/api/types.gen';

async function createDrug(drug: Partial<Drug>): Promise<Drug> {
  const { data } = await axiosClient.post<Drug>('/drugs', drug);
  return data;
}

async function updateDrug({ id, ...drug }: Partial<Drug> & { id: string }): Promise<Drug> {
  const { data } = await axiosClient.put<Drug>(`/drugs/${id}`, drug);
  return data;
}

async function deleteDrug(id: string): Promise<void> {
  await axiosClient.delete(`/drugs/${id}`);
}

export function useCreateDrug() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createDrug,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['drugs'] }),
  });
}

export function useUpdateDrug() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateDrug,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['drugs'] }),
  });
}

export function useDeleteDrug() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteDrug,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['drugs'] }),
  });
}
