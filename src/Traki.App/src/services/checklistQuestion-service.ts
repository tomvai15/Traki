import axiosApiInstance from './axios-instance';
import { GetChecklistQuestionsResponse } from '../contracts/checklistQuestion/GetChecklistQuestionsResponse';

const route = 'checklists/{checklistId}/checklistquestions/{checklistsQuestionId}'

class ChecklistService {
  async getChecklistQuestions(checklistId: number): Promise<GetChecklistQuestionsResponse> {
    const fullRoute = route.format({checklistId: checklistId.toString(), checklistsQuestionId: ''}) 
    const response = await axiosApiInstance.get<GetChecklistQuestionsResponse>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new ChecklistService ();