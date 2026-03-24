import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Quotation, PurchaseOrder } from '@/api/localTypes';

async function deleteQuotation(id: string): Promise<void> {
  await axiosClient.delete(`/quotations/${id}`);
}

async function acceptQuotation({
  id,
  branchId,
}: {
  id: string;
  branchId?: string | null;
}): Promise<PurchaseOrder> {
  const { data } = await axiosClient.post<PurchaseOrder>(
    `/quotations/${id}/accept`,
    null,
    { params: branchId ? { branchId } : {} },
  );
  return data;
}

async function rejectQuotation(id: string): Promise<Quotation> {
  const { data } = await axiosClient.put<Quotation>(`/quotations/${id}`, { status: 'Rejected' });
  return data;
}

export function useDeleteQuotation() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteQuotation,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['quotations'] }),
  });
}

export function useAcceptQuotation() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: acceptQuotation,
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['quotations'] });
      qc.invalidateQueries({ queryKey: ['purchase-orders'] });
    },
  });
}

export function useRejectQuotation() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: rejectQuotation,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['quotations'] }),
  });
}
