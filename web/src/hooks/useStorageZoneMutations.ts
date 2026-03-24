import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { StorageZone } from './useStorageZones';

type SavePayload = Partial<StorageZone>;

async function createStorageZone(data: SavePayload): Promise<StorageZone> {
  const res = await axiosClient.post<StorageZone>('/storage-zones', data);
  return res.data;
}

async function updateStorageZone({ id, ...data }: SavePayload & { id: string }): Promise<StorageZone> {
  const res = await axiosClient.put<StorageZone>(`/storage-zones/${id}`, { id, ...data });
  return res.data;
}

async function deleteStorageZone(id: string): Promise<void> {
  await axiosClient.delete(`/storage-zones/${id}`);
}

export function useCreateStorageZone() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createStorageZone,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['storage-zones'] }),
  });
}

export function useUpdateStorageZone() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateStorageZone,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['storage-zones'] }),
  });
}

export function useDeleteStorageZone() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteStorageZone,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['storage-zones'] }),
  });
}
