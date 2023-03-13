import axiosApiInstance from './axios-instance';
import { GetChecklistQuestionsResponse } from '../contracts/checklistQuestion/GetChecklistQuestionsResponse';
import { UpdateChecklistQuestionsRequest } from '../contracts/checklistQuestion/UpdateChecklistQuestionsRequest';

const route = 'checklists/{checklistId}/checklistquestions/{checklistsQuestionId}'

class ChecklistService {
  async getChecklistQuestions(checklistId: number): Promise<GetChecklistQuestionsResponse> {
    const fullRoute = route.format({checklistId: checklistId.toString(), checklistsQuestionId: ''}) 
    const response = await axiosApiInstance.get<GetChecklistQuestionsResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async updateChecklists(checklistId: number, updateChecklistQuestionsRequest: UpdateChecklistQuestionsRequest): Promise<void> {
    const fullRoute = route.format({checklistId: checklistId.toString(), checklistsQuestionId:''}) 
    await axiosApiInstance.put(fullRoute, updateChecklistQuestionsRequest, { headers: {} });
  }
}
export default new ChecklistService ();