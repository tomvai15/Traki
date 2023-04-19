import { GetDefectNotificationsResponse } from 'contracts/drawing/defect/GetDefectNotificationsResponse';
import axiosApiInstance from './axios-instance';

const route = 'notifications/{defectId}';

class NotificationService {
  async getNotifications(): Promise<GetDefectNotificationsResponse> {
    const fullRoute = route.format({defectId: ''}); 
    const response = await axiosApiInstance.get<GetDefectNotificationsResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async deleteNotifications(defectId: number): Promise<void> {
    const fullRoute = route.format({defectId: defectId.toString()}); 
    await axiosApiInstance.delete(fullRoute, { headers: {} });
  }
}
export default new NotificationService ();