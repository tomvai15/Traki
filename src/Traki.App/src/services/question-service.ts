import axiosApiInstance from './axios-instance';
import { GetQuestionsResponse } from '../contracts/question/GetQuestionsResponse';
import { GetQuestionResponse } from '../contracts/question/GetQuestionResponse';
import { UpdateQuestionRequest } from '../contracts/question/UpdateQuestionRequest';

const route = 'templates/{templateId}/questions/{questionId}'

class QuestionService {
  async getQuestions(templateId: number): Promise<GetQuestionsResponse> {
    const fullRoute = route.format({templateId: templateId.toString(), questionId: ''}) 
    const response = await axiosApiInstance.get<GetQuestionsResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async getQuestion(templateId: number, questionId: number): Promise<GetQuestionResponse> {
    const fullRoute = route.format({templateId: templateId.toString(), questionId: questionId.toString()}) 
    const response = await axiosApiInstance.get<GetQuestionResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async updateQuestion(templateId: number, questionId: number, updateQuestionRequest: UpdateQuestionRequest): Promise<void> {
    const fullRoute = route.format({templateId: templateId.toString(), questionId: questionId.toString()}) 
    await axiosApiInstance.put(fullRoute, updateQuestionRequest, { headers: {} });
  }
}
export default new QuestionService ();