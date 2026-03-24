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

export interface MonthlySalesReport {
  year: number;
  month: number;
  invoiceCount: number;
  subTotal: number;
  totalDiscount: number;
  totalCgst: number;
  totalSgst: number;
  totalIgst: number;
  gstAmount: number;
  netAmount: number;
}

async function fetchMonthlySales(year: number, month: number): Promise<MonthlySalesReport> {
  const { data } = await axiosClient.get<MonthlySalesReport>('/reports/monthly-sales', {
    params: { year, month },
  });
  return data;
}

export function useMonthlySalesReport(year: number, month: number) {
  return useQuery({
    queryKey: ['reports', 'monthly-sales', year, month],
    queryFn: () => fetchMonthlySales(year, month),
    staleTime: 1000 * 60 * 10,
    retry: false,
  });
}

export interface StockValuationItem {
  drugId: string;
  drugName: string | null;
  hsnCode: string | null;
  totalQuantity: number;
  mrp: number;
  totalMrpValue: number;
  averageCostPrice: number | null;
  totalCostValue: number | null;
}

async function fetchStockValuation(): Promise<StockValuationItem[]> {
  const { data } = await axiosClient.get<StockValuationItem[]>('/reports/stock-valuation');
  return data;
}

export function useStockValuationReport() {
  return useQuery({
    queryKey: ['reports', 'stock-valuation'],
    queryFn: fetchStockValuation,
    staleTime: 1000 * 60 * 5,
    retry: false,
  });
}
