import axios from 'axios';

// Lazy import to avoid circular dependency — globalStore imports nothing from api/
let getAuthToken: () => string | null = () => null;
let onUnauthorized: () => void = () => {};

export function configureAxiosClient(
  tokenGetter: () => string | null,
  unauthorizedHandler: () => void,
) {
  getAuthToken = tokenGetter;
  onUnauthorized = unauthorizedHandler;
}

const axiosClient = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
  timeout: 15_000,
});

axiosClient.interceptors.request.use((config) => {
  const token = getAuthToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

axiosClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (axios.isAxiosError(error) && error.response?.status === 401) {
      onUnauthorized();
    }
    return Promise.reject(error);
  },
);

export default axiosClient;
