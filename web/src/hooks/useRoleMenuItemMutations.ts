import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { RoleMenuItem } from './useRoleMenuItems';

export function useCreateRoleMenuItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: { roleId: string; menuItemId: string }) =>
      axiosClient.post<RoleMenuItem>('/role-menu-items', payload).then((r) => r.data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['role-menu-items'] }),
  });
}

export function useDeleteRoleMenuItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => axiosClient.delete(`/role-menu-items/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['role-menu-items'] }),
  });
}
