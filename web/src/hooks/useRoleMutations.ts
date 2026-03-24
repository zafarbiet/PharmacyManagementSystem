import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Role } from '@/api/localTypes';

async function createRole(payload: Partial<Role>): Promise<Role> {
  const { data } = await axiosClient.post<Role>('/roles', payload);
  return data;
}

async function updateRole({ id, ...payload }: Partial<Role> & { id: string }): Promise<Role> {
  const { data } = await axiosClient.put<Role>(`/roles/${id}`, payload);
  return data;
}

async function deleteRole(id: string): Promise<void> {
  await axiosClient.delete(`/roles/${id}`);
}

export function useCreateRole() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createRole,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['roles'] }),
  });
}

export function useUpdateRole() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateRole,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['roles'] }),
  });
}

export function useDeleteRole() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteRole,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['roles'] }),
  });
}
