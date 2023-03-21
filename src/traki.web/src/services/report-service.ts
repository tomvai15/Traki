import axiosApiInstance from './axios-instance';

const route = 'reports';

class ChecklistService {
  async getReport(): Promise<string> {
    const fullRoute = route;
    const response = await axiosApiInstance.get<string>(fullRoute, { headers: {} });
    return response.data;
  }

  async signReport(): Promise<string> {
    const fullRoute = route + '/sign';
    const response = await axiosApiInstance.post<string>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new ChecklistService ();