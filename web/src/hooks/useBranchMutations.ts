import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { Branch } from '@/api/localTypes';

async function createBranch(branch: Partial<Branch>): Promise<Branch> {
  const { data } = await axiosClient.post<Branch>('/branches', branch);
  return data;
}

async function updateBranch({ id, ...branch }: Partial<Branch> & { id: string }): Promise<Branch> {
  const { data } = await axiosClient.put<Branch>(`/branches/${id}`, branch);
  return data;
}

async function deleteBranch(id: string): Promise<void> {
  await axiosClient.delete(`/branches/${id}`);
}

export function useCreateBranch() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createBranch,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['branches'] }),
  });
}

export function useUpdateBranch() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateBranch,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['branches'] }),
  });
}

export function useDeleteBranch() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteBranch,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['branches'] }),
  });
}
