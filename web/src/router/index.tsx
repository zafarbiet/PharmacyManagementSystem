import { createBrowserRouter, Navigate } from 'react-router-dom';
import AppLayout from '@/layout/AppLayout';
import LoginPage from '@/pages/LoginPage';
import DashboardPage from '@/pages/dashboard/DashboardPage';
import DrugInventoryListPage from '@/pages/inventory/DrugInventoryListPage';
import DrugsListPage from '@/pages/drugs/DrugsListPage';
import VendorsListPage from '@/pages/vendors/VendorsListPage';
import PurchaseOrdersListPage from '@/pages/purchase-orders/PurchaseOrdersListPage';
import ExpiryManagementPage from '@/pages/expiry/ExpiryManagementPage';
import ReportsPage from '@/pages/reports/ReportsPage';
import InvoicesListPage from '@/pages/invoices/InvoicesListPage';
import UsersListPage from '@/pages/users/UsersListPage';
import BranchesListPage from '@/pages/settings/BranchesListPage';
import DrugCategoriesListPage from '@/pages/settings/DrugCategoriesListPage';
import RolesListPage from '@/pages/settings/RolesListPage';
import StorageZonesPage from '@/pages/settings/StorageZonesPage';
import MenuConfigPage from '@/pages/settings/MenuConfigPage';
import QuotationRequestsListPage from '@/pages/quotations/QuotationRequestsListPage';
import QuotationsListPage from '@/pages/quotations/QuotationsListPage';
import PatientsListPage from '@/pages/patients/PatientsListPage';
import PatientPrescriptionsPage from '@/pages/patients/PatientPrescriptionsPage';
import DebtRecordsListPage from '@/pages/debt/DebtRecordsListPage';
import ManufacturersListPage from '@/pages/settings/ManufacturersListPage';
import PromotionsListPage from '@/pages/settings/PromotionsListPage';
import QuotationVendorResponsesPage from '@/pages/quotations/QuotationVendorResponsesPage';
import SubscriptionsListPage from '@/pages/subscriptions/SubscriptionsListPage';
import DamageRecordsPage from '@/pages/inventory/DamageRecordsPage';
import NotificationsPage from '@/pages/notifications/NotificationsPage';
import PaymentLedgerPage from '@/pages/payments/PaymentLedgerPage';
import DailyDiaryPage from '@/pages/diary/DailyDiaryPage';
import AuditLogsPage from '@/pages/audit-logs/AuditLogsPage';
import RacksListPage from '@/pages/settings/RacksListPage';
import DrugPricingPage from '@/pages/settings/DrugPricingPage';
import StockTransactionsPage from '@/pages/inventory/StockTransactionsPage';
import FrontDeskPage from '@/pages/front-desk/FrontDeskPage';
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
        element: <DashboardPage />,
      },
      {
        path: 'inventory/drugs',
        element: <DrugInventoryListPage />,
      },
      {
        path: 'drugs',
        element: <DrugsListPage />,
      },
      {
        path: 'vendors',
        element: <VendorsListPage />,
      },
      {
        path: 'purchase-orders',
        element: <PurchaseOrdersListPage />,
      },
      {
        path: 'expired-drugs',
        element: <ExpiryManagementPage />,
      },
      {
        path: 'invoices',
        element: <InvoicesListPage />,
      },
      {
        path: 'reports',
        element: <ReportsPage />,
      },
      {
        path: 'users',
        element: <UsersListPage />,
      },
      {
        path: 'settings/branches',
        element: <BranchesListPage />,
      },
      {
        path: 'settings/drug-categories',
        element: <DrugCategoriesListPage />,
      },
      {
        path: 'settings/roles',
        element: <RolesListPage />,
      },
      {
        path: 'settings/storage',
        element: <StorageZonesPage />,
      },
      {
        path: 'settings/menu',
        element: <MenuConfigPage />,
      },
      {
        path: 'patients',
        element: <PatientsListPage />,
      },
      {
        path: 'patients/:patientId/prescriptions',
        element: <PatientPrescriptionsPage />,
      },
      {
        path: 'debt',
        element: <DebtRecordsListPage />,
      },
      {
        path: 'subscriptions',
        element: <SubscriptionsListPage />,
      },
      {
        path: 'inventory/damage',
        element: <DamageRecordsPage />,
      },
      {
        path: 'notifications',
        element: <NotificationsPage />,
      },
      {
        path: 'quotations/rfq',
        element: <QuotationRequestsListPage />,
      },
      {
        path: 'quotations',
        element: <QuotationsListPage />,
      },
      {
        path: 'quotations/vendor-responses',
        element: <QuotationVendorResponsesPage />,
      },
      {
        path: 'settings/manufacturers',
        element: <ManufacturersListPage />,
      },
      {
        path: 'settings/promotions',
        element: <PromotionsListPage />,
      },
      {
        path: 'payments',
        element: <PaymentLedgerPage />,
      },
      {
        path: 'diary',
        element: <DailyDiaryPage />,
      },
      {
        path: 'audit-logs',
        element: <AuditLogsPage />,
      },
      {
        path: 'settings/racks',
        element: <RacksListPage />,
      },
      {
        path: 'settings/drug-pricing',
        element: <DrugPricingPage />,
      },
      {
        path: 'inventory/stock-transactions',
        element: <StockTransactionsPage />,
      },
      {
        path: 'front-desk',
        element: <FrontDeskPage />,
      },
    ],
  },
  {
    path: '*',
    element: <Navigate to="/" replace />,
  },
]);
