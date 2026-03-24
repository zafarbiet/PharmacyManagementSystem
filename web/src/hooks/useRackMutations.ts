import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Rack } from './useRacks';

type SavePayload = Partial<Rack>;

async function createRack(data: SavePayload): Promise<Rack> {
  const res = await axiosClient.post<Rack>('/racks', data);
  return res.data;
}

async function updateRack({ id, ...data }: SavePayload & { id: string }): Promise<Rack> {
  const res = await axiosClient.put<Rack>(`/racks/${id}`, { id, ...data });
  return res.data;
}

async function deleteRack(id: string): Promise<void> {
  await axiosClient.delete(`/racks/${id}`);
}

export function useCreateRack() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createRack,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['racks'] }),
  });
}

export function useUpdateRack() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateRack,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['racks'] }),
  });
}

export function useDeleteRack() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteRack,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['racks'] }),
  });
}
