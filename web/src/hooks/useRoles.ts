import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Role } from '@/api/localTypes';

async function fetchRoles(): Promise<Role[]> {
  const { data } = await axiosClient.get<Role[]>('/roles');
  return data;
}

export function useRoles() {
  return useQuery({
    queryKey: ['roles'],
    queryFn: fetchRoles,
    staleTime: 1000 * 60 * 5,
  });
}
