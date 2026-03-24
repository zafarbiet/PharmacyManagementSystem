import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';

export interface CustomerSubscription {
  id: string;
  patientId: string;
  startDate: string;
  endDate: string | null;
  cycleDayOfMonth: number;
  status: string | null;
  approvedBy: string | null;
  approvedAt: string | null;
  approvalStatus: string | null;
  notes: string | null;
  isActive: boolean;
  updatedAt: string;
  updatedBy: string | null;
}

export interface CustomerSubscriptionItem {
  id: string;
  subscriptionId: string;
  drugId: string;
  quantityPerCycle: number;
  prescriptionId: string | null;
  isActive: boolean;
}

async function fetchSubscriptions(params: { patientId?: string; status?: string; approvalStatus?: string }): Promise<CustomerSubscription[]> {
  const { data } = await axiosClient.get<CustomerSubscription[]>('/customer-subscriptions', { params });
  return data;
}

async function fetchSubscriptionItems(subscriptionId: string): Promise<CustomerSubscriptionItem[]> {
  const { data } = await axiosClient.get<CustomerSubscriptionItem[]>('/customer-subscription-items', {
    params: { subscriptionId },
  });
  return data;
}

export function useSubscriptions(params: { patientId?: string; status?: string; approvalStatus?: string } = {}) {
  return useQuery({
    queryKey: ['subscriptions', params],
    queryFn: () => fetchSubscriptions(params),
    staleTime: 1000 * 60 * 2,
  });
}

export function useSubscriptionItems(subscriptionId: string | undefined) {
  return useQuery({
    queryKey: ['subscription-items', subscriptionId],
    queryFn: () => fetchSubscriptionItems(subscriptionId!),
    enabled: !!subscriptionId,
    staleTime: 1000 * 60 * 2,
  });
}
