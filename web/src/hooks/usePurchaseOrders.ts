import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { PurchaseOrder } from '@/api/types.gen';

export interface PurchaseOrderQueryParams {
  vendorId?: string;
  status?: string;
  dateFrom?: string;
  dateTo?: string;
}

async function fetchPurchaseOrders(params: PurchaseOrderQueryParams): Promise<PurchaseOrder[]> {
  const { data } = await axiosClient.get<PurchaseOrder[]>('/purchase-orders', { params });
  return data;
}

export function usePurchaseOrders(params: PurchaseOrderQueryParams = {}) {
  return useQuery({
    queryKey: ['purchase-orders', params],
    queryFn: () => fetchPurchaseOrders(params),
    staleTime: 1000 * 60 * 2,
  });
}
