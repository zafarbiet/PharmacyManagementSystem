import { createBrowserRouter, Navigate } from 'react-router-dom';
import AppLayout from '@/layout/AppLayout';
import LoginPage from '@/pages/LoginPage';
import DrugInventoryListPage from '@/pages/inventory/DrugInventoryListPage';
import { useGlobalStore } from '@/store/globalStore';

function RequireAuth({ children }: { children: React.ReactNode }) {
  const authToken = useGlobalStore((s) => s.authToken);
  if (!authToken) return <Navigate to="/login" replace />;
  return <>{children}</>;
}

export const router = createBrowserRouter([
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    path: '/',
    element: (
      <RequireAuth>
        <AppLayout />
      </RequireAuth>
    ),
    children: [
      {
        index: true,
        element: <Navigate to="/inventory/drugs" replace />,
      },
      {
        path: 'inventory/drugs',
        element: <DrugInventoryListPage />,
      },
      // Phase 2+ routes added here
    ],
  },
  {
    path: '*',
    element: <Navigate to="/" replace />,
  },
]);
