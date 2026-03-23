import { Tag, Tooltip } from 'antd';
import dayjs from 'dayjs';
import relativeTime from 'dayjs/plugin/relativeTime';

dayjs.extend(relativeTime);

// Mirrors SaveDrugInventoryAction.DefaultExpiryThresholdDays = 90
const WARN_DAYS = 90;

interface ExpiryTagProps {
  expirationDate: string;
}

function getExpiryStatus(expirationDate: string): 'expired' | 'expiring' | 'safe' {
  const now = dayjs();
  const expiry = dayjs(expirationDate);
  if (expiry.isBefore(now)) return 'expired';
  if (expiry.diff(now, 'day') <= WARN_DAYS) return 'expiring';
  return 'safe';
}

const statusConfig = {
  expired:  { color: 'red',    label: 'Expired' },
  expiring: { color: 'orange', label: 'Expiring Soon' },
  safe:     { color: 'green',  label: 'OK' },
} as const;

export default function ExpiryTag({ expirationDate }: ExpiryTagProps) {
  const status = getExpiryStatus(expirationDate);
  const { color, label } = statusConfig[status];
  const formatted = dayjs(expirationDate).format('DD MMM YYYY');
  const relative = dayjs(expirationDate).fromNow();

  return (
    <Tooltip title={`${formatted} (${relative})`}>
      <Tag color={color}>
        {label}: {formatted}
      </Tag>
    </Tooltip>
  );
}
