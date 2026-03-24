import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DrugCategory } from '@/api/types.gen';

async function fetchDrugCategories(): Promise<DrugCategory[]> {
  const { data } = await axiosClient.get<DrugCategory[]>('/drug-categories');
  return data;
}

export function useDrugCategories() {
  return useQuery({
    queryKey: ['drug-categories'],
    queryFn: fetchDrugCategories,
    staleTime: 1000 * 60 * 10,
  });
}
