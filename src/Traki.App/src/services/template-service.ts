import axios from 'axios';
import { url } from './endpoints';
import { GetTemplatesResponse } from '../contracts/template/GetTemplatesResponse';
import { GetTemplateResponse } from '../contracts/template/GetTemplateResponse';

const route = url + 'templates'

class TemplateService {
  async getTemplates(): Promise<GetTemplatesResponse> {
    const response = await axios.get<GetTemplatesResponse>(route, { headers: {} });
    return response.data;
  }

  async getTemplate(id: number): Promise<GetTemplateResponse> {
    const response = await axios.get<GetTemplateResponse>(`${route}/${id}`, { headers: {} });
    return response.data;
  }
}
export default new TemplateService ();