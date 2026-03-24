import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { AppUser } from '@/api/types.gen';

async function fetchAppUsers(): Promise<AppUser[]> {
  const { data } = await axiosClient.get<AppUser[]>('/app-users');
  return data;
}

export function useAppUsers() {
  return useQuery({
    queryKey: ['app-users'],
    queryFn: fetchAppUsers,
    staleTime: 1000 * 60 * 5,
  });
}
