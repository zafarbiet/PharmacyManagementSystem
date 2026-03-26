import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { MenuItem } from './useMenuItems';

type MenuItemPayload = Omit<MenuItem, 'id' | 'updatedAt' | 'updatedBy' | 'isActive'>;

export function useCreateMenuItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: MenuItemPayload) =>
      axiosClient.post<MenuItem>('/menu-items', payload).then((r) => r.data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['menu-items'] }),
  });
}

export function useUpdateMenuItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, ...payload }: MenuItemPayload & { id: string }) =>
      axiosClient.put<MenuItem>(`/menu-items/${id}`, payload).then((r) => r.data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['menu-items'] }),
  });
}

export function useDeleteMenuItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => axiosClient.delete(`/menu-items/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['menu-items'] }),
  });
}
