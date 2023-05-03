import { formatDistanceToNow } from 'date-fns';

function formatDate(date: Date): string {
  return date.toDateString();
}

function formatTimeDifference(pastDate: Date): string {
  const currentDate = new Date();
  return formatDistanceToNow(pastDate, { addSuffix: true });
}

export { formatDate, formatTimeDifference };