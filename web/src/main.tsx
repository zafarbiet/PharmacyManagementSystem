import React from 'react';
import ReactDOM from 'react-dom/client';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { configureAxiosClient } from '@/api/axiosClient';
import { useGlobalStore } from '@/store/globalStore';
import App from './App';
import './index.css';

// Wire up axiosClient to read auth token from Zustand without circular imports
configureAxiosClient(
  () => useGlobalStore.getState().authToken,
  () => useGlobalStore.getState().clearAuth(),
);

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 2,
      retry: 1,
    },
  },
});

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <App />
      <ReactQueryDevtools initialIsOpen={false} />
    </QueryClientProvider>
  </React.StrictMode>,
);
