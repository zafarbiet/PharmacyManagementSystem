import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { UserRole } from '@/api/localTypes';

async function fetchUserRoles(userId?: string, roleId?: string): Promise<UserRole[]> {
  const { data } = await axiosClient.get<UserRole[]>('/user-roles', {
    params: { userId, roleId },
  });
  return data;
}

export function useUserRoles(userId?: string, roleId?: string) {
  return useQuery({
    queryKey: ['user-roles', userId, roleId],
    queryFn: () => fetchUserRoles(userId, roleId),
    enabled: !!(userId || roleId),
    staleTime: 1000 * 60 * 2,
  });
}
