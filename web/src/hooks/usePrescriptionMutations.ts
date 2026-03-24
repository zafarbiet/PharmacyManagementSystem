import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Prescription, PrescriptionItem } from '@/api/localTypes';

async function createPrescription(
  payload: Partial<Prescription> & { items: Partial<PrescriptionItem>[] },
): Promise<Prescription> {
  const { items, ...header } = payload;
  const { data: rx } = await axiosClient.post<Prescription>('/prescriptions', header);
  if (rx?.id && items.length > 0) {
    await Promise.all(
      items.map((item) =>
        axiosClient.post('/prescription-items', { ...item, prescriptionId: rx.id }),
      ),
    );
  }
  return rx;
}

async function updatePrescription({ id, ...payload }: Partial<Prescription> & { id: string }): Promise<Prescription> {
  const { data } = await axiosClient.put<Prescription>(`/prescriptions/${id}`, payload);
  return data;
}

async function deletePrescription(id: string): Promise<void> {
  await axiosClient.delete(`/prescriptions/${id}`);
}

export function useCreatePrescription() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createPrescription,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['prescriptions'] }),
  });
}

export function useUpdatePrescription() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updatePrescription,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['prescriptions'] }),
  });
}

export function useDeletePrescription() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deletePrescription,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['prescriptions'] }),
  });
}
