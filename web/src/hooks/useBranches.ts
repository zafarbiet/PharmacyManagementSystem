import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Branch } from '@/api/localTypes';

async function fetchBranches(): Promise<Branch[]> {
  const { data } = await axiosClient.get<Branch[]>('/branches');
  return data;
}

export function useBranches() {
  return useQuery({
    queryKey: ['branches'],
    queryFn: fetchBranches,
    staleTime: 1000 * 60 * 5,
  });
}
