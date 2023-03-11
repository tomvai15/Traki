import axiosApiInstance from './axios-instance';
import { store } from '../store/store';
import { GetChecklistsResponse } from '../contracts/checklist/GetChecklistsResponse';
import { GetChecklistResponse } from '../contracts/checklist/GetChecklistResponse';

const route = 'products/{productId}/checklists/{checklistId}'

class ChecklistService {
  async getChecklists(productId: number): Promise<GetChecklistsResponse> {
    const fullRoute = route.format({productId: productId.toString(), checklistId:''}) 
    const response = await axiosApiInstance.get<GetChecklistsResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async getChecklist(productId: number, checklistId: number): Promise<GetChecklistResponse> {
    const projectId = store.getState().project.id;
    const fullRoute = route.format({productId: productId.toString(), checklistId: checklistId.toString()}) 
    const response = await axiosApiInstance.get<GetChecklistResponse>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new ChecklistService ();