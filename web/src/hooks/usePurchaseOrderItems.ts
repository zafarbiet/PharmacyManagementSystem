import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { PurchaseOrderItem } from '@/api/localTypes';

async function fetchPurchaseOrderItems(purchaseOrderId: string): Promise<PurchaseOrderItem[]> {
  const { data } = await axiosClient.get<PurchaseOrderItem[]>('/purchase-order-items', {
    params: { purchaseOrderId },
  });
  return data;
}

export function usePurchaseOrderItems(purchaseOrderId: string | undefined) {
  return useQuery({
    queryKey: ['purchase-order-items', purchaseOrderId],
    queryFn: () => fetchPurchaseOrderItems(purchaseOrderId!),
    enabled: !!purchaseOrderId,
    staleTime: 1000 * 60 * 2,
  });
}
