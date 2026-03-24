import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';

export interface DamageRecord {
  id: string;
  drugInventoryId: string;
  quantityDamaged: number;
  damageType: string | null;
  damagedAt: string;
  discoveredBy: string | null;
  status: string | null;
  quarantineRackId: string | null;
  stockTransactionId: string | null;
  approvedBy: string | null;
  approvedAt: string | null;
  notes: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

async function fetchDamageRecords(params: { status?: string }): Promise<DamageRecord[]> {
  const { data } = await axiosClient.get<DamageRecord[]>('/damage-records', { params });
  return data;
}
async function createDamageRecord(rec: Partial<DamageRecord>): Promise<DamageRecord> {
  const { data } = await axiosClient.post<DamageRecord>('/damage-records', rec);
  return data;
}
async function deleteDamageRecord(id: string): Promise<void> {
  await axiosClient.delete(`/damage-records/${id}`);
}

export function useDamageRecords(params: { status?: string } = {}) {
  return useQuery({
    queryKey: ['damage-records', params],
    queryFn: () => fetchDamageRecords(params),
    staleTime: 1000 * 60 * 2,
  });
}
export function useCreateDamageRecord() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createDamageRecord,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['damage-records'] }),
  });
}
export function useDeleteDamageRecord() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteDamageRecord,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['damage-records'] }),
  });
}
