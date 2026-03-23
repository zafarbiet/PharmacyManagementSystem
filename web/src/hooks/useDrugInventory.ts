import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DrugInventory, DrugInventoryFilter } from '@/api/localTypes';

async function fetchDrugInventory(params: DrugInventoryFilter): Promise<DrugInventory[]> {
  const { data } = await axiosClient.get<DrugInventory[]>('/drug-inventories', { params });
  return data;
}

export function useDrugInventory(filter: DrugInventoryFilter = {}) {
  return useQuery({
    queryKey: ['drug-inventory', filter],
    queryFn: () => fetchDrugInventory(filter),
    staleTime: 1000 * 60,
  });
}
