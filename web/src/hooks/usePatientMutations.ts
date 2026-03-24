import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Patient } from '@/api/localTypes';

async function createPatient(payload: Partial<Patient>): Promise<Patient> {
  const { data } = await axiosClient.post<Patient>('/patients', payload);
  return data;
}

async function updatePatient({ id, ...payload }: Partial<Patient> & { id: string }): Promise<Patient> {
  const { data } = await axiosClient.put<Patient>(`/patients/${id}`, payload);
  return data;
}

async function deletePatient(id: string): Promise<void> {
  await axiosClient.delete(`/patients/${id}`);
}

export function useCreatePatient() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createPatient,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['patients'] }),
  });
}

export function useUpdatePatient() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updatePatient,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['patients'] }),
  });
}

export function useDeletePatient() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deletePatient,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['patients'] }),
  });
}
