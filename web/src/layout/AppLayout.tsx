import { useEffect } from 'react';
import { Layout } from 'antd';
import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';
import Topbar from './Topbar';
import axiosClient from '@/api/axiosClient';
import { useGlobalStore } from '@/store/globalStore';
import type { Branch } from '@/api/types.gen';

const { Content } = Layout;

export default function AppLayout() {
  const { availableBranches, setAvailableBranches, setSelectedBranch } = useGlobalStore();

  useEffect(() => {
    if (availableBranches.length > 0) return; // already loaded
    axiosClient.get<Branch[]>('/branches').then(({ data }) => {
      setAvailableBranches(data);
      const head = data.find((b) => b.isHeadOffice) ?? data[0];
      if (head) setSelectedBranch(head);
    }).catch(() => {
      // silently ignore — branches are optional for the UI to function
    });
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sidebar />
      <Layout>
        <Topbar />
        <Content
          style={{
            margin: 16,
            padding: '16px 24px',
            background: '#fff',
            borderRadius: 8,
          }}
        >
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
}
