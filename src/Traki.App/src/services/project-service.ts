import { GetProjectsResponse } from '../contracts/projects/GetProjectsResponse';
import { GetProjectResponse } from '../contracts/projects/GetProjectResponse';
import axios from 'axios';
import { CreateProjectRequest } from '../contracts/projects/CreateProjectRequest';

// TODO: use https
const url = 'http://10.0.2.2:5219/api/projects';

class ProjectService {
  async getProjects(): Promise<GetProjectsResponse> {
    const response = await axios.get<GetProjectsResponse>(url, { headers: {} });
    return Object.create(response.data) as GetProjectsResponse;
  }

  async getProject(id: number): Promise<GetProjectResponse> {
    const response = await axios.get<GetProjectResponse>(`${url}/${id}`, { headers: {} });
    return response.data;
  }

  async createProject(createProjectRequest: CreateProjectRequest): Promise<boolean> {
    await axios.post(url, createProjectRequest, { headers: {} });
    return true;
  }
}
export default new ProjectService ();