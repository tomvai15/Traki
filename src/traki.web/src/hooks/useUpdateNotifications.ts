import { useRecoilState } from "recoil";
import { notificationService } from "services";
import { notificationsState } from "state/notification-state";

export const useUpdateNotifications = () => {

  const [, setNotifications] = useRecoilState(notificationsState);
  
  const updateNotifications = async () => {
    const response = await notificationService.getNotifications();

    setNotifications(response.defectNotifications);
  };

  return { updateNotifications };
};