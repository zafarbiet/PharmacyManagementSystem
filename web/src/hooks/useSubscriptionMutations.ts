import { useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { CustomerSubscription, CustomerSubscriptionItem } from './useSubscriptions';

async function createSubscription(sub: Partial<CustomerSubscription>): Promise<CustomerSubscription> {
  const { data } = await axiosClient.post<CustomerSubscription>('/customer-subscriptions', sub);
  return data;
}

async function updateSubscription({ id, ...sub }: Partial<CustomerSubscription> & { id: string }): Promise<CustomerSubscription> {
  const { data } = await axiosClient.put<CustomerSubscription>(`/customer-subscriptions/${id}`, sub);
  return data;
}

async function deleteSubscription(id: string): Promise<void> {
  await axiosClient.delete(`/customer-subscriptions/${id}`);
}

async function approveSubscription({ id, approvedBy }: { id: string; approvedBy: string }): Promise<CustomerSubscription> {
  const { data } = await axiosClient.post<CustomerSubscription>(`/customer-subscriptions/${id}/approve`, null, {
    params: { approvedBy },
  });
  return data;
}

async function createSubscriptionItem(item: Partial<CustomerSubscriptionItem>): Promise<CustomerSubscriptionItem> {
  const { data } = await axiosClient.post<CustomerSubscriptionItem>('/customer-subscription-items', item);
  return data;
}

async function deleteSubscriptionItem(id: string): Promise<void> {
  await axiosClient.delete(`/customer-subscription-items/${id}`);
}

export function useCreateSubscription() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createSubscription,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['subscriptions'] }),
  });
}

export function useUpdateSubscription() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: updateSubscription,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['subscriptions'] }),
  });
}

export function useDeleteSubscription() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteSubscription,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['subscriptions'] }),
  });
}

export function useApproveSubscription() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: approveSubscription,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['subscriptions'] }),
  });
}

export function useCreateSubscriptionItem() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: createSubscriptionItem,
    onSuccess: (_, vars) => qc.invalidateQueries({ queryKey: ['subscription-items', vars.subscriptionId] }),
  });
}

export function useDeleteSubscriptionItem() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteSubscriptionItem,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['subscription-items'] }),
  });
}
