import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';

export type Rack = {
  id: string;
  storageZoneId: string;
  label: string | null;
  description: string | null;
  capacity: number | null;
  updatedAt: string;
  updatedBy: string | null;
  isActive: boolean;
};

type Filter = { storageZoneId?: string; label?: string };

async function fetchRacks(filter: Filter): Promise<Rack[]> {
  const { data } = await axiosClient.get<Rack[]>('/racks', { params: filter });
  return data;
}

export function useRacks(filter: Filter = {}) {
  return useQuery({
    queryKey: ['racks', filter],
    queryFn: () => fetchRacks(filter),
  });
}
