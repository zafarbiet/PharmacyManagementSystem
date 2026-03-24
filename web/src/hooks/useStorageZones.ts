import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';

export type StorageZone = {
  id: string;
  name: string | null;
  zoneType: string | null;
  description: string | null;
  temperatureRangeMin: number | null;
  temperatureRangeMax: number | null;
  updatedAt: string;
  updatedBy: string | null;
  isActive: boolean;
};

type Filter = { name?: string; zoneType?: string };

async function fetchStorageZones(filter: Filter): Promise<StorageZone[]> {
  const { data } = await axiosClient.get<StorageZone[]>('/storage-zones', { params: filter });
  return data;
}

export function useStorageZones(filter: Filter = {}) {
  return useQuery({
    queryKey: ['storage-zones', filter],
    queryFn: () => fetchStorageZones(filter),
  });
}
