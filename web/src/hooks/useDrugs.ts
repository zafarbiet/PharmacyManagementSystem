import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Drug } from '@/api/types.gen';

export interface DrugQueryParams {
  name?: string;
  genericName?: string;
  categoryId?: string;
  composition?: string;
}

async function fetchDrugs(params: DrugQueryParams): Promise<Drug[]> {
  const { data } = await axiosClient.get<Drug[]>('/drugs', { params });
  return data;
}

export function useDrugs(params: DrugQueryParams = {}) {
  return useQuery({
    queryKey: ['drugs', params],
    queryFn: () => fetchDrugs(params),
    staleTime: 1000 * 60 * 2,
  });
}
