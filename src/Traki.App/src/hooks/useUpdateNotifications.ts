import { useRecoilState } from 'recoil';
import { notificationsState } from '../state/notification-state';
import { notificationService } from '../services';

export const useUpdateNotifications = () => {

  const [, setNotifications] = useRecoilState(notificationsState);
  
  const updateNotifications = async () => {
    const response = await notificationService.getNotifications();

    setNotifications(response.defectNotifications);
  };

  return { updateNotifications };
};