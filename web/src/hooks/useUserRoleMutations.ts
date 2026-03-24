import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { UserRole } from '@/api/localTypes';

async function createUserRole(payload: Partial<UserRole>): Promise<UserRole> {
  const { data } = await axiosClient.post<UserRole>('/user-roles', payload);
  return data;
}

async function deleteUserRole(id: string): Promise<void> {
  await axiosClient.delete(`/user-roles/${id}`);
}

export function useAssignRole() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createUserRole,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['user-roles'] }),
  });
}

export function useRevokeRole() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteUserRole,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['user-roles'] }),
  });
}
