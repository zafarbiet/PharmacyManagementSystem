import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Vendor } from '@/api/types.gen';

export interface VendorQueryParams {
  name?: string;
}

async function fetchVendors(params: VendorQueryParams): Promise<Vendor[]> {
  const { data } = await axiosClient.get<Vendor[]>('/vendors', { params });
  return data;
}

export function useVendors(params: VendorQueryParams = {}) {
  return useQuery({
    queryKey: ['vendors', params],
    queryFn: () => fetchVendors(params),
    staleTime: 1000 * 60 * 2,
  });
}
