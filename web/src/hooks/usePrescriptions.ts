import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Prescription, PrescriptionItem } from '@/api/localTypes';

async function fetchPrescriptions(patientId?: string): Promise<Prescription[]> {
  const { data } = await axiosClient.get<Prescription[]>('/prescriptions', {
    params: patientId ? { patientId } : {},
  });
  return data;
}

async function fetchPrescriptionItems(prescriptionId: string): Promise<PrescriptionItem[]> {
  const { data } = await axiosClient.get<PrescriptionItem[]>('/prescription-items', {
    params: { prescriptionId },
  });
  return data;
}

export function usePrescriptions(patientId?: string) {
  return useQuery({
    queryKey: ['prescriptions', patientId],
    queryFn: () => fetchPrescriptions(patientId),
    staleTime: 1000 * 60 * 2,
  });
}

export function usePrescriptionItems(prescriptionId: string) {
  return useQuery({
    queryKey: ['prescription-items', prescriptionId],
    queryFn: () => fetchPrescriptionItems(prescriptionId),
    staleTime: 1000 * 60 * 2,
    enabled: !!prescriptionId,
  });
}
