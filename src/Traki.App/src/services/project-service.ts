import axios from 'axios';
import { GetProjectsResponse } from '../contracts/projects/GetProjectsResponse';
import { GetProjectResponse } from '../contracts/projects/GetProjectResponse';
import { CreateProjectRequest } from '../contracts/projects/CreateProjectRequest';
import { url } from './endpoints';

const route = url + 'projects'

class ProjectService {
  async getProjects(): Promise<GetProjectsResponse> {
    const response = await axios.get<GetProjectsResponse>(route, { headers: {} });
    return Object.create(response.data) as GetProjectsResponse;
  }

  async getProject(id: number): Promise<GetProjectResponse> {
    const response = await axios.get<GetProjectResponse>(`${route}/${id}`, { headers: {} });
    return response.data;
  }

  async createProject(createProjectRequest: CreateProjectRequest): Promise<boolean> {
    await axios.post(route, createProjectRequest, { headers: {} });
    return true;
  }
}
export default new ProjectService ();