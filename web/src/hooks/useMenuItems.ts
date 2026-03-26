import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';

export type MenuItem = {
  id: string;
  key: string | null;
  label: string | null;
  icon: string | null;
  parentKey: string | null;
  orderIndex: number;
  updatedAt: string;
  updatedBy: string | null;
  isActive: boolean;
};

async function fetchMenuItems(params: Record<string, string> = {}): Promise<MenuItem[]> {
  const { data } = await axiosClient.get<MenuItem[]>('/menu-items', { params });
  return data;
}

export function useMenuItems(params?: Record<string, string>) {
  return useQuery({
    queryKey: ['menu-items', params],
    queryFn: () => fetchMenuItems(params),
    staleTime: 1000 * 60 * 5,
  });
}

async function fetchMenuItemsForUser(username: string): Promise<MenuItem[]> {
  const { data } = await axiosClient.get<MenuItem[]>('/menu-items/for-user', {
    params: { username },
  });
  return data;
}

export function useMenuItemsForUser(username: string | null | undefined) {
  return useQuery({
    queryKey: ['menu-items', 'for-user', username],
    queryFn: () => fetchMenuItemsForUser(username!),
    enabled: !!username,
    staleTime: 1000 * 60 * 5,
  });
}
