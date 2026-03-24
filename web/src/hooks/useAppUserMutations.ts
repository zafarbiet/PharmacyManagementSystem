import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { AppUser } from '@/api/types.gen';

async function createAppUser(user: Partial<AppUser>): Promise<AppUser> {
  const { data } = await axiosClient.post<AppUser>('/app-users', user);
  return data;
}

async function updateAppUser({ id, ...user }: Partial<AppUser> & { id: string }): Promise<AppUser> {
  const { data } = await axiosClient.put<AppUser>(`/app-users/${id}`, user);
  return data;
}

async function deleteAppUser(id: string): Promise<void> {
  await axiosClient.delete(`/app-users/${id}`);
}

export function useCreateAppUser() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createAppUser,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['app-users'] }),
  });
}

export function useUpdateAppUser() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateAppUser,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['app-users'] }),
  });
}

export function useDeleteAppUser() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteAppUser,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['app-users'] }),
  });
}
