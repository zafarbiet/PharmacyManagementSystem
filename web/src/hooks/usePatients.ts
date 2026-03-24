import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Patient } from '@/api/localTypes';

async function fetchPatients(name?: string, contactNumber?: string): Promise<Patient[]> {
  const { data } = await axiosClient.get<Patient[]>('/patients', {
    params: { name, contactNumber },
  });
  return data;
}

export function usePatients(name?: string, contactNumber?: string) {
  return useQuery({
    queryKey: ['patients', name, contactNumber],
    queryFn: () => fetchPatients(name, contactNumber),
    staleTime: 1000 * 60 * 2,
  });
}
