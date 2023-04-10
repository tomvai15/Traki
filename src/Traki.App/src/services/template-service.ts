import axiosApiInstance from './axios-instance';
import { GetTemplatesResponse } from '../contracts/template/GetTemplatesResponse';
import { GetTemplateResponse } from '../contracts/template/GetTemplateResponse';

const route = 'projects/{projectId}/templates/{templateId}';

class TemplateService {
  async getTemplates(): Promise<GetTemplatesResponse> {
    const projectId = 1;
    const fullRoute = route.format({projectId: projectId.toString(), templateId:''}); 
    const response = await axiosApiInstance.get<GetTemplatesResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async getTemplate(id: number): Promise<GetTemplateResponse> {
    const projectId = 1;
    const fullRoute = route.format({projectId: projectId.toString(), templateId: id.toString()}); 
    const response = await axiosApiInstance.get<GetTemplateResponse>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new TemplateService ();