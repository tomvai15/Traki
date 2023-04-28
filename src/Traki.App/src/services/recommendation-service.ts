import axiosApiInstance from './axios-instance';
import { GetRecommendationResponse } from '../contracts/recommendation/GetRecommendationResponse';

const route = 'recommendations';

class RecommendationService {
  async getRecommendations(): Promise<GetRecommendationResponse> {
    const response = await axiosApiInstance.get<GetRecommendationResponse>(route, { headers: {} });
    return response.data;
  }
}
export default new RecommendationService ();