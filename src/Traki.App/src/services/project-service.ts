import { GetProjectResponse } from '../contracts/GetProjectsReponse';
import ApiService from './api-service';

class ProjectService {
  async get(): Promise<Array<GetProjectResponse>> {
    return ApiService.get<Array<GetProjectResponse>>('projects');
  }
}
export default new ProjectService ();