import { useQuery } from '@tanstack/react-query';
import axiosClient from '@/api/axiosClient';
import type { DailySalesReport } from '@/api/localTypes';

async function fetchDailySales(date: string): Promise<DailySalesReport> {
  const { data } = await axiosClient.get<DailySalesReport>('/reports/daily-sales', {
    params: { date },
  });
  return data;
}

export function useDailySalesReport(date: string) {
  return useQuery({
    queryKey: ['reports', 'daily-sales', date],
    queryFn: () => fetchDailySales(date),
    staleTime: 1000 * 60 * 5,
    retry: false,
  });
}
