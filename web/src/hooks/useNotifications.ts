import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';

export interface AppNotification {
  id: string;
  notificationType: string | null;
  channel: string | null;
  recipientType: string | null;
  recipientId: string;
  subject: string | null;
  body: string | null;
  referenceId: string | null;
  referenceType: string | null;
  scheduledAt: string;
  sentAt: string | null;
  status: string | null;
  failureReason: string | null;
  retryCount: number;
  isActive: boolean;
  updatedAt: string;
}

async function fetchNotifications(params: {
  notificationType?: string;
  status?: string;
  channel?: string;
}): Promise<AppNotification[]> {
  const { data } = await axiosClient.get<AppNotification[]>('/notifications', { params });
  return data;
}

async function deleteNotification(id: string): Promise<void> {
  await axiosClient.delete(`/notifications/${id}`);
}

export function useNotifications(params: { notificationType?: string; status?: string; channel?: string } = {}) {
  return useQuery({
    queryKey: ['notifications', params],
    queryFn: () => fetchNotifications(params),
    staleTime: 1000 * 30,
  });
}

export function useDeleteNotification() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: deleteNotification,
    onSuccess: () => qc.invalidateQueries({ queryKey: ['notifications'] }),
  });
}
