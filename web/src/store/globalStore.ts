import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import type { AppUser, Branch } from '@/api/types.gen';

interface GlobalState {
  currentUser: AppUser | null;
  authToken: string | null;
  selectedBranch: Branch | null;
  availableBranches: Branch[];

  setCurrentUser: (user: AppUser | null) => void;
  setAuthToken: (token: string | null) => void;
  clearAuth: () => void;
  setSelectedBranch: (branch: Branch) => void;
  setAvailableBranches: (branches: Branch[]) => void;
}

export const useGlobalStore = create<GlobalState>()(
  persist(
    (set) => ({
      currentUser: null,
      authToken: null,
      selectedBranch: null,
      availableBranches: [],

      setCurrentUser: (user) => set({ currentUser: user }),
      setAuthToken: (token) => set({ authToken: token }),
      clearAuth: () => set({ currentUser: null, authToken: null, selectedBranch: null }),
      setSelectedBranch: (branch) => set({ selectedBranch: branch }),
      setAvailableBranches: (branches) => set({ availableBranches: branches }),
    }),
    {
      name: 'pms-global-store',
      partialize: (state) => ({
        currentUser: state.currentUser,
        authToken: state.authToken,
        selectedBranch: state.selectedBranch,
      }),
    },
  ),
);
