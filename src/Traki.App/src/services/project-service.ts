import { GetProjectsResponse } from '../contracts/projects/GetProjectsResponse';
import { GetProjectResponse } from '../contracts/projects/GetProjectResponse';
import axios from 'axios';
import { API_BASE } from '@env';

// TODO: use https
const url = 'http://10.0.2.2:5219/projects';

class ProjectService {
  async getProjects(): Promise<GetProjectsResponse> {
    const response = await axios.get<GetProjectsResponse>(url, { headers: {} });
    return Object.create(response.data) as GetProjectsResponse;
  }

  async getProject(id: number): Promise<GetProjectResponse> {
    const response = await axios.get<GetProjectResponse>(`${url}/${id}`, { headers: {} });
    return response.data;
  }
}
export default new ProjectService ();