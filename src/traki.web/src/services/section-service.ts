import { UpdateSectionRequest } from '../contracts/protocol/UpdateSectionRequest';
import axiosApiInstance from './axios-instance';

const route = 'sections/1';

class SectionService {
  async getReport(): Promise<string> {
    const fullRoute = route;
    const response = await axiosApiInstance.get<string>(fullRoute, { headers: {} });
    return response.data;
  }

  async updateSection(updateSectionRequest: UpdateSectionRequest): Promise<void> {
    const fullRoute = route;
    await axiosApiInstance.put(fullRoute, updateSectionRequest, { headers: {} });
  }
}
export default new SectionService ();