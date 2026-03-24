import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DrugCategory } from '@/api/types.gen';

async function createDrugCategory(cat: Partial<DrugCategory>): Promise<DrugCategory> {
  const { data } = await axiosClient.post<DrugCategory>('/drug-categories', cat);
  return data;
}

async function updateDrugCategory({ id, ...cat }: Partial<DrugCategory> & { id: string }): Promise<DrugCategory> {
  const { data } = await axiosClient.put<DrugCategory>(`/drug-categories/${id}`, cat);
  return data;
}

async function deleteDrugCategory(id: string): Promise<void> {
  await axiosClient.delete(`/drug-categories/${id}`);
}

export function useCreateDrugCategory() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createDrugCategory,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['drug-categories'] }),
  });
}

export function useUpdateDrugCategory() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateDrugCategory,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['drug-categories'] }),
  });
}

export function useDeleteDrugCategory() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteDrugCategory,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['drug-categories'] }),
  });
}
