import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { PurchaseOrder, PurchaseOrderItem } from '@/api/localTypes';

async function createPurchaseOrder(po: Partial<PurchaseOrder>): Promise<PurchaseOrder> {
  const { data } = await axiosClient.post<PurchaseOrder>('/purchase-orders', po);
  return data;
}

async function updatePurchaseOrder({ id, ...po }: Partial<PurchaseOrder> & { id: string }): Promise<PurchaseOrder> {
  const { data } = await axiosClient.put<PurchaseOrder>(`/purchase-orders/${id}`, po);
  return data;
}

async function deletePurchaseOrder(id: string): Promise<void> {
  await axiosClient.delete(`/purchase-orders/${id}`);
}

async function approvePurchaseOrder({ id, approvedBy }: { id: string; approvedBy: string }): Promise<PurchaseOrder> {
  const { data } = await axiosClient.post<PurchaseOrder>(`/purchase-orders/${id}/approve`, null, {
    params: { approvedBy },
  });
  return data;
}

async function rejectPurchaseOrder({ id, rejectedBy }: { id: string; rejectedBy: string }): Promise<PurchaseOrder> {
  const { data } = await axiosClient.post<PurchaseOrder>(`/purchase-orders/${id}/reject`, null, {
    params: { rejectedBy },
  });
  return data;
}

async function createPurchaseOrderItem(item: Partial<PurchaseOrderItem>): Promise<PurchaseOrderItem> {
  const { data } = await axiosClient.post<PurchaseOrderItem>('/purchase-order-items', item);
  return data;
}

async function receiveConsignment({
  id,
  items,
}: {
  id: string;
  items: Partial<PurchaseOrderItem>[];
}): Promise<PurchaseOrder> {
  const { data } = await axiosClient.post<PurchaseOrder>(`/purchase-orders/${id}/receive`, items);
  return data;
}

export function useCreatePurchaseOrder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createPurchaseOrder,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['purchase-orders'] }),
  });
}

export function useUpdatePurchaseOrder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updatePurchaseOrder,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['purchase-orders'] }),
  });
}

export function useDeletePurchaseOrder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deletePurchaseOrder,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['purchase-orders'] }),
  });
}

export function useApprovePurchaseOrder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: approvePurchaseOrder,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['purchase-orders'] }),
  });
}

export function useRejectPurchaseOrder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: rejectPurchaseOrder,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['purchase-orders'] }),
  });
}

export function useCreatePurchaseOrderItem() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createPurchaseOrderItem,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['purchase-orders'] }),
  });
}

export function useReceiveConsignment() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: receiveConsignment,
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['purchase-orders'] });
      qc.invalidateQueries({ queryKey: ['drug-inventory'] });
    },
  });
}
