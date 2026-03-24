import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { QuotationRequest, QuotationRequestItem } from '@/api/localTypes';

async function createQuotationRequest(
  payload: Partial<QuotationRequest> & { items: Partial<QuotationRequestItem>[] },
): Promise<QuotationRequest> {
  const { items, ...header } = payload;
  const { data: qr } = await axiosClient.post<QuotationRequest>('/quotation-requests', header);
  if (qr?.id && items.length > 0) {
    await Promise.all(
      items.map((item) =>
        axiosClient.post('/quotation-request-items', { ...item, quotationRequestId: qr.id }),
      ),
    );
  }
  return qr;
}

async function deleteQuotationRequest(id: string): Promise<void> {
  await axiosClient.delete(`/quotation-requests/${id}`);
}

async function dispatchQuotationRequest({
  id,
  vendorIds,
}: {
  id: string;
  vendorIds: string[];
}): Promise<QuotationRequest> {
  const { data } = await axiosClient.post<QuotationRequest>(
    `/quotation-requests/${id}/dispatch`,
    vendorIds,
  );
  return data;
}

export function useCreateQuotationRequest() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createQuotationRequest,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['quotation-requests'] }),
  });
}

export function useDeleteQuotationRequest() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteQuotationRequest,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['quotation-requests'] }),
  });
}

export function useDispatchQuotationRequest() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: dispatchQuotationRequest,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['quotation-requests'] }),
  });
}
