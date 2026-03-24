import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DrugInventory } from '@/api/localTypes';

async function createDrugInventory(inv: Partial<DrugInventory>): Promise<DrugInventory> {
  const { data } = await axiosClient.post<DrugInventory>('/drug-inventories', inv);
  return data;
}

async function updateDrugInventory({ id, ...inv }: Partial<DrugInventory> & { id: string }): Promise<DrugInventory> {
  const { data } = await axiosClient.put<DrugInventory>(`/drug-inventories/${id}`, inv);
  return data;
}

async function deleteDrugInventory(id: string): Promise<void> {
  await axiosClient.delete(`/drug-inventories/${id}`);
}

export function useCreateDrugInventory() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createDrugInventory,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['drug-inventories'] }),
  });
}

export function useUpdateDrugInventory() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateDrugInventory,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['drug-inventories'] }),
  });
}

export function useDeleteDrugInventory() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteDrugInventory,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['drug-inventories'] }),
  });
}
