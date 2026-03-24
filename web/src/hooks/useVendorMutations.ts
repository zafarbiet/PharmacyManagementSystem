import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Vendor } from '@/api/types.gen';

async function createVendor(vendor: Partial<Vendor>): Promise<Vendor> {
  const { data } = await axiosClient.post<Vendor>('/vendors', vendor);
  return data;
}

async function updateVendor({ id, ...vendor }: Partial<Vendor> & { id: string }): Promise<Vendor> {
  const { data } = await axiosClient.put<Vendor>(`/vendors/${id}`, vendor);
  return data;
}

async function deleteVendor(id: string): Promise<void> {
  await axiosClient.delete(`/vendors/${id}`);
}

export function useCreateVendor() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createVendor,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['vendors'] }),
  });
}

export function useUpdateVendor() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateVendor,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['vendors'] }),
  });
}

export function useDeleteVendor() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteVendor,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['vendors'] }),
  });
}
