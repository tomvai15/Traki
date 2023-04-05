import { CreateProjectRequest } from '../contracts/projects/CreateProjectRequest';
import { GetProjectResponse } from '../contracts/projects/GetProjectResponse';
import { GetProjectsResponse } from '../contracts/projects/GetProjectsResponse';
import axiosApiInstance from './axios-instance';

const route = 'projects';

class ProjectService {
  async getProjects(): Promise<GetProjectsResponse> {
    const response = await axiosApiInstance.get<GetProjectsResponse>(route, { headers: {} });
    return Object.create(response.data) as GetProjectsResponse;
  }

  async getProject(id: number): Promise<GetProjectResponse> {
    const response = await axiosApiInstance.get<GetProjectResponse>(`${route}/${id}`, { headers: {} });
    return response.data;
  }

  async createProject(createProjectRequest: CreateProjectRequest): Promise<boolean> {
    await axiosApiInstance.post(route, createProjectRequest, { headers: {} });
    return true;
  }

  async updateProject(projectId: number, createProjectRequest: CreateProjectRequest): Promise<boolean> {
    await axiosApiInstance.put(route + '/' + projectId, createProjectRequest, { headers: {} });
    return true;
  }
}
export default new ProjectService ();