import { GetSectionResponse } from '../contracts/protocol/GetSectionResponse';
import { GetSectionsResponse } from '../contracts/protocol/GetSectionsResponse';
import { UpdateSectionRequest } from '../contracts/protocol/UpdateSectionRequest';
import axiosApiInstance from './axios-instance';

const route = 'protocols/{protocolId}/sections/{sectionId}';

class SectionService {
  async getReport(): Promise<string> {
    const fullRoute = route;
    const response = await axiosApiInstance.get<string>(fullRoute, { headers: {} });
    return response.data;
  }

  async getSections(protocolId: number): Promise<GetSectionsResponse> {
    const fullRoute = route.format({protocolId: protocolId.toString(), sectionId: ''}); 
    const response = await axiosApiInstance.get<GetSectionsResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async getSection(protocolId: number, sectionId: number): Promise<GetSectionResponse> {
    const fullRoute = route.format({protocolId: protocolId.toString(), sectionId: sectionId.toString()}); 
    const response = await axiosApiInstance.get<GetSectionResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async deleteSection(protocolId: number, sectionId: number): Promise<void> {
    const fullRoute = route.format({protocolId: protocolId.toString(), sectionId: sectionId.toString()}); 
    await axiosApiInstance.delete<GetSectionResponse>(fullRoute, { headers: {} });
  }

  async createSection(protocolId: number, updateSectionRequest: UpdateSectionRequest): Promise<void> {
    const fullRoute = route.format({protocolId: protocolId.toString(), sectionId: ''}); 
    await axiosApiInstance.post<GetSectionResponse>(fullRoute, updateSectionRequest, { headers: {} });
  }

  async updateSection(protocolId: number, sectionId: number, updateSectionRequest: UpdateSectionRequest): Promise<void> {
    const fullRoute = route.format({protocolId: protocolId.toString(), sectionId: sectionId.toString()}); 
    await axiosApiInstance.put(fullRoute, updateSectionRequest, { headers: {} });
  }
}
export default new SectionService ();