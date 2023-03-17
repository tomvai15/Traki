import axiosApiInstance from './axios-instance';

const route = 'reports';

class ChecklistService {
    async getReport(): Promise<string> {
        const fullRoute = route;
        const response = await axiosApiInstance.get<string>(fullRoute, { headers: {} });
        return response.data;
    }
}
export default new ChecklistService ();