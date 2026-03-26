import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';

export type RoleMenuItem = {
  id: string;
  roleId: string;
  menuItemId: string;
  updatedAt: string;
  updatedBy: string | null;
  isActive: boolean;
};

async function fetchRoleMenuItems(params: Record<string, string>): Promise<RoleMenuItem[]> {
  const { data } = await axiosClient.get<RoleMenuItem[]>('/role-menu-items', { params });
  return data;
}

export function useRoleMenuItems(params: Record<string, string>) {
  return useQuery({
    queryKey: ['role-menu-items', params],
    queryFn: () => fetchRoleMenuItems(params),
    staleTime: 1000 * 60 * 2,
  });
}
